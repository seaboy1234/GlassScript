using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassScript.Language.Syntax;

namespace GlassScript.Language.Parser
{
    public sealed partial class GlassScriptParser
    {
        private bool _error;
        private ErrorSink _errorSink;
        private int _index;
        private GlassScriptParserOptions _options;
        private SourceCode _sourceCode;
        private IEnumerable<Token> _tokens;

        private Token _current => _tokens.ElementAtOrDefault(_index) ?? _tokens.Last();

        private Token _last => Peek(-1);

        private Token _next => Peek(1);

        public GlassScriptParser()
            : this(new ErrorSink())
        {
        }

        public GlassScriptParser(ErrorSink errorSink)
        {
            _errorSink = errorSink;
        }

        private void AddError(Severity severity, string message, SourceSpan? span = null)
        {
            _errorSink.AddError(message, _sourceCode, severity, span ?? CreateSpan(_current));
        }

        private void Advance()
        {
            _index++;
        }

        private SourceSpan CreateSpan(SourceLocation start, SourceLocation end)
        {
            return new SourceSpan(start, end);
        }

        private SourceSpan CreateSpan(Token start)
        {
            return CreateSpan(start.Span.Start, _current.Span.End);
        }

        private SourceSpan CreateSpan(SyntaxNode start)
        {
            return CreateSpan(start.Span.Start, _current.Span.End);
        }

        private SourceSpan CreateSpan(SourceLocation start)
        {
            return CreateSpan(start, _current.Span.End);
        }

        private void InitializeParser(SourceCode sourceCode, IEnumerable<Token> tokens, GlassScriptParserOptions options)
        {
            _sourceCode = sourceCode;
            _tokens = tokens.Where(g => !g.IsTrivia()).ToArray();
            _options = options;
            _index = 0;
        }

        private void MakeBlock(Action action, TokenKind openKind = TokenKind.LeftBracket, TokenKind closeKind = TokenKind.RightBracket)
        {
            Take(openKind);

            MakeStatement(action, closeKind);
        }

        private void MakeStatement(Action action, TokenKind closeKind = TokenKind.Semicolon)
        {
            try
            {
                while (_current != closeKind && _current != TokenKind.EndOfFile)
                {
                    action();
                }
            }
            catch (SyntaxException)
            {
                while (_current != closeKind && _current != TokenKind.EndOfFile)
                {
                    Take();
                }
            }
            finally
            {
                if (_error)
                {
                    if (_last == closeKind)
                    {
                        _index--;
                    }
                    if (_current != closeKind)
                    {
                        while (_current != closeKind && _current != TokenKind.EndOfFile)
                        {
                            Take();
                        }
                    }
                    _error = false;
                }
                if (closeKind == TokenKind.Semicolon)
                {
                    TakeSemicolon();
                }
                else
                {
                    Take(closeKind);
                }
            }
        }

        private Token Peek(int ahead)
        {
            return _tokens.ElementAtOrDefault(_index + ahead) ?? _tokens.Last();
        }

        private SyntaxException SyntaxError(Severity severity, string message, SourceSpan? span = null)
        {
            _error = true;
            AddError(severity, message, span);
            return new SyntaxException(message);
        }

        private Token Take()
        {
            var token = _current;
            Advance();
            return token;
        }

        private Token Take(TokenKind kind)
        {
            if (_current != kind)
            {
                throw UnexpectedToken(kind);
            }
            return Take();
        }

        private Token Take(string contextualKeyword)
        {
            if (_current != TokenKind.Identifier && _current != contextualKeyword)
            {
                throw UnexpectedToken(contextualKeyword);
            }
            return Take();
        }

        private Token TakeKeyword(string keyword)
        {
            if (_current != TokenKind.Keyword && _current != keyword)
            {
                throw UnexpectedToken(keyword);
            }
            return Take();
        }

        private Token TakeSemicolon()
        {
            if (_options.EnforceSemicolons || _current == TokenKind.Semicolon)
            {
                return Take(TokenKind.Semicolon);
            }
            return _current;
        }

        private SyntaxException UnexpectedToken(TokenKind expected)
        {
            return UnexpectedToken(expected.ToString());
        }

        private SyntaxException UnexpectedToken(string expected)
        {
            Advance();
            var value = string.IsNullOrEmpty(_last?.Value) ? _last?.Kind.ToString() : _last?.Value;
            string message = $"Unexpected '{value}'.  Expected '{expected}'";

            return SyntaxError(Severity.Error, message, _last.Span);
        }
    }
}
