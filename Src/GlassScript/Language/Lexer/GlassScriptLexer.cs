using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Lexer
{
    public sealed class GlassScriptLexer
    {
        private static readonly string[] _Keywords = { "class", "func", "new", "if", "else", "switch", "case", "default", "do", "while", "for", "var", "null" };

        private StringBuilder _builder;
        private int _column;
        private ErrorSink _errorSink;
        private int _index;
        private int _line;
        private string[] _lines;
        private string _sourceCode;
        private SourceLocation _tokenStart;

        public ErrorSink ErrorSink => _errorSink;

        private char _ch => _sourceCode.CharAt(_index);

        private char _last => Peek(-1);

        private char _next => Peek(1);

        public GlassScriptLexer()
            : this(new ErrorSink())
        {
        }

        public GlassScriptLexer(ErrorSink errorSink)
        {
            _errorSink = errorSink;
            _builder = new StringBuilder();
            _sourceCode = "";
        }

        public IEnumerable<Token> LexFile(string sourceCode)
        {
            _sourceCode = sourceCode;
            _lines = _sourceCode.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            _builder.Clear();
            _column = 0;
            _line = 1;
            _index = 0;
            _tokenStart = new SourceLocation(_index, _last, _column);

            return LexContents();
        }

        private void AddError(string message, Severity severity)
        {
            var span = new SourceSpan(_tokenStart, new SourceLocation(_index, _line, _column));
            _errorSink.AddError(message, _lines[_line - 1], severity, span);
        }

        private void Advance()
        {
            _index++;
            _column++;
        }

        private void Consume()
        {
            _builder.Append(_ch);
            Advance();
        }

        private Token CreateToken(TokenKind kind)
        {
            string contents = _builder.ToString();
            SourceLocation end = new SourceLocation(_index, _line, _column);
            SourceLocation start = _tokenStart;

            _tokenStart = end;
            _builder.Clear();

            return new Token(kind, contents, start, end);
        }

        private void DoNewLine()
        {
            _line++;
            _column = 0;
        }

        private bool IsDigit()
        {
            return char.IsDigit(_ch);
        }

        private bool IsEOF()
        {
            return _ch == '\0';
        }

        private bool IsIdentifier()
        {
            return IsLetterOrDigit() || _ch == '_';
        }

        private bool IsKeyword()
        {
            return _Keywords.Contains(_builder.ToString());
        }

        private bool IsLetter()
        {
            return char.IsLetter(_ch);
        }

        private bool IsLetterOrDigit()
        {
            return char.IsLetterOrDigit(_ch);
        }

        private bool IsNewLine()
        {
            return _ch == '\n';
        }

        private bool IsPunctuation()
        {
            return "<>{}()[]!%^&*+-=/.,?;:|".Contains(_ch);
        }

        private bool IsWhiteSpace()
        {
            return (char.IsWhiteSpace(_ch) || IsEOF()) && !IsNewLine();
        }

        private IEnumerable<Token> LexContents()
        {
            while (!IsEOF())
            {
                yield return LexToken();
            }
        }

        private Token LexToken()
        {
            if (IsEOF())
            {
                return CreateToken(TokenKind.EndOfFile);
            }
            else if (IsNewLine())
            {
                return ScanNewLine();
            }
            else if (IsWhiteSpace())
            {
                return ScanWhiteSpace();
            }
            else if (IsDigit())
            {
                return ScanInteger();
            }
            else if (_ch == '/' && (_next == '/' || _next == '*'))
            {
                return ScanComment();
            }
            else if (IsLetter() || _ch == '_')
            {
                return ScanIdentifier();
            }
            else if (_ch == '"')
            {
                return ScanStringLiteral();
            }
            else if (_ch == '.' && char.IsDigit(_next))
            {
                return ScanFloat();
            }
            else if (IsPunctuation())
            {
                return ScanPunctuation();
            }
            else
            {
                return ScanWord();
            }
        }

        private char Peek(int ahead)
        {
            return _sourceCode.CharAt(_index + ahead);
        }

        private Token ScanBlockComment()
        {
            Func<bool> isEndOfComment = () => _ch == '*' && _next == '/';
            while (!isEndOfComment())
            {
                if (IsEOF())
                {
                    return CreateToken(TokenKind.Error);
                }
                if (IsNewLine())
                {
                    DoNewLine();
                }
                Consume();
            }

            Consume();
            Consume();

            return CreateToken(TokenKind.BlockComment);
        }

        private Token ScanComment()
        {
            Consume();
            if (_ch == '*')
            {
                return ScanBlockComment();
            }

            Consume();

            while (!IsNewLine() && !IsEOF())
            {
                Consume();
            }

            return CreateToken(TokenKind.LineComment);
        }

        private Token ScanFloat()
        {
            if (_ch == 'f')
            {
                Advance();
                if ((!IsWhiteSpace() && !IsPunctuation() && !IsEOF()) || _ch == '.')
                {
                    return ScanWord(message: "Remove 'f' in floating point number.");
                }
                return CreateToken(TokenKind.FloatLiteral);
            }
            int preDotLength = _index - _tokenStart.Index;

            if (_ch == '.')
            {
                Consume();
            }
            while (IsDigit())
            {
                Consume();
            }

            if (_last == '.')
            {
                // .e10 is invalid.
                return ScanWord(message: "Must contain digits after '.'");
            }

            if (_ch == 'e')
            {
                Consume();
                if (preDotLength > 1)
                {
                    return ScanWord(message: "Coefficient must be less than 10.");
                }

                if (_ch == '+' || _ch == '-')
                {
                    Consume();
                }
                while (IsDigit())
                {
                    Consume();
                }
            }

            if (_ch == 'f')
            {
                Consume();
            }

            if (!IsWhiteSpace() && !IsPunctuation() && !IsEOF())
            {
                if (IsLetter())
                {
                    return ScanWord(message: "'{0}' is an invalid float value");
                }
                return ScanWord();
            }

            return CreateToken(TokenKind.FloatLiteral);
        }

        private Token ScanIdentifier()
        {
            while (IsIdentifier())
            {
                Consume();
            }

            if (!IsWhiteSpace() && !IsPunctuation() && !IsEOF())
            {
                return ScanWord();
            }

            if (IsKeyword())
            {
                return CreateToken(TokenKind.Keyword);
            }

            return CreateToken(TokenKind.Identifier);
        }

        private Token ScanInteger()
        {
            while (IsDigit())
            {
                Consume();
            }

            if (_ch == 'f' || _ch == '.' || _ch == 'e')
            {
                return ScanFloat();
            }

            if (!IsWhiteSpace() && !IsPunctuation() && !IsEOF())
            {
                return ScanWord();
            }

            return CreateToken(TokenKind.IntegerLiteral);
        }

        private Token ScanNewLine()
        {
            Consume();

            DoNewLine();

            return CreateToken(TokenKind.NewLine);
        }

        private Token ScanPunctuation()
        {
            switch (_ch)
            {
                case ';':
                    Consume();
                    return CreateToken(TokenKind.Semicolon);

                case ':':
                    Consume();
                    return CreateToken(TokenKind.Colon);

                case '{':
                    Consume();
                    return CreateToken(TokenKind.LeftBracket);

                case '}':
                    Consume();
                    return CreateToken(TokenKind.RightBracket);

                case '[':
                    Consume();
                    return CreateToken(TokenKind.LeftBrace);

                case ']':
                    Consume();
                    return CreateToken(TokenKind.RightBrace);

                case '(':
                    Consume();
                    return CreateToken(TokenKind.LeftParenthesis);

                case ')':
                    Consume();
                    return CreateToken(TokenKind.RightParenthesis);

                case '>':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.GreaterThanOrEqual);
                    }
                    else if (_ch == '>')
                    {
                        Consume();
                        return CreateToken(TokenKind.BitShiftRight);
                    }
                    return CreateToken(TokenKind.GreaterThan);

                case '<':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.LessThanOrEqual);
                    }
                    else if (_ch == '<')
                    {
                        Consume();
                        return CreateToken(TokenKind.BitShiftLeft);
                    }
                    return CreateToken(TokenKind.LessThan);

                case '+':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.PlusEqual);
                    }
                    else if (_ch == '+')
                    {
                        Consume();
                        return CreateToken(TokenKind.PlusPlus);
                    }
                    return CreateToken(TokenKind.Plus);

                case '-':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.MinusEqual);
                    }
                    else if (_ch == '>')
                    {
                        Consume();
                        return CreateToken(TokenKind.Arrow);
                    }
                    else if (_ch == '-')
                    {
                        Consume();
                        return CreateToken(TokenKind.MinusMinus);
                    }
                    return CreateToken(TokenKind.Minus);

                case '=':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.Equal);
                    }
                    else if (_ch == '>')
                    {
                        Consume();
                        return CreateToken(TokenKind.FatArrow);
                    }
                    return CreateToken(TokenKind.Assignment);

                case '!':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.NotEqual);
                    }
                    return CreateToken(TokenKind.Not);

                case '*':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.MulEqual);
                    }
                    return CreateToken(TokenKind.Mul);

                case '/':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.DivEqual);
                    }
                    return CreateToken(TokenKind.Div);

                case '.':
                    Consume();
                    return CreateToken(TokenKind.Dot);

                case ',':
                    Consume();
                    return CreateToken(TokenKind.Comma);

                case '&':
                    Consume();
                    if (_ch == '&')
                    {
                        Consume();
                        return CreateToken(TokenKind.BooleanAnd);
                    }
                    else if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.BitwiseAndEqual);
                    }
                    return CreateToken(TokenKind.BitwiseAnd);

                case '|':
                    Consume();
                    if (_ch == '|')
                    {
                        Consume();
                        return CreateToken(TokenKind.BooleanOr);
                    }
                    else if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.BitwiseOrEqual);
                    }
                    return CreateToken(TokenKind.BitwiseOr);

                case '%':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.ModEqual);
                    }
                    return CreateToken(TokenKind.Mod);

                case '^':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.BitwiseXorEqual);
                    }
                    return CreateToken(TokenKind.BitwiseXor);

                case '?':
                    Consume();
                    if (_ch == '?')
                    {
                        Consume();
                        return CreateToken(TokenKind.DoubleQuestion);
                    }

                    return CreateToken(TokenKind.Question);

                default: return ScanWord();
            }
        }

        private Token ScanStringLiteral()
        {
            Advance();

            while (_ch != '"')
            {
                if (IsEOF())
                {
                    AddError("Unexpected End Of File", Severity.Fatal);
                    return CreateToken(TokenKind.Error);
                }
                Consume();
            }

            Advance();

            return CreateToken(TokenKind.StringLiteral);
        }

        private Token ScanWhiteSpace()
        {
            while (IsWhiteSpace())
            {
                Consume();
            }
            return CreateToken(TokenKind.WhiteSpace);
        }

        private Token ScanWord(Severity severity = Severity.Error, string message = "Unexpected Token '{0}'")
        {
            while (!IsWhiteSpace() && !IsEOF() && !IsPunctuation())
            {
                Consume();
            }
            AddError(string.Format(message, _builder.ToString()), severity);
            return CreateToken(TokenKind.Error);
        }
    }
}
