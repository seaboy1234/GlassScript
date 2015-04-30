using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language
{
    public sealed class Token : IEquatable<Token>
    {
        private Lazy<TokenCatagory> _catagory;

        public TokenCatagory Catagory => _catagory.Value;

        public TokenKind Kind { get; }

        public SourceSpan Span { get; }

        public string Value { get; }

        public Token(TokenKind kind, string contents, SourceLocation start, SourceLocation end)
        {
            Kind = kind;
            Value = contents;
            Span = new SourceSpan(start, end);

            _catagory = new Lazy<TokenCatagory>(GetTokenCatagory);
        }

        public static bool operator !=(Token left, string right)
        {
            return left?.Value != right;
        }

        public static bool operator !=(string left, Token right)
        {
            return right?.Value != left;
        }

        public static bool operator !=(Token left, TokenKind right)
        {
            return left?.Kind != right;
        }

        public static bool operator !=(TokenKind left, Token right)
        {
            return right?.Kind != left;
        }

        public static bool operator ==(Token left, string right)
        {
            return left?.Value == right;
        }

        public static bool operator ==(string left, Token right)
        {
            return right?.Value == left;
        }

        public static bool operator ==(Token left, TokenKind right)
        {
            return left?.Kind == right;
        }

        public static bool operator ==(TokenKind left, Token right)
        {
            return right?.Kind == left;
        }

        public override bool Equals(object obj)
        {
            if (obj is Token)
            {
                return Equals((Token)obj);
            }
            return base.Equals(obj);
        }

        public bool Equals(Token other)
        {
            if (other == null)
            {
                return false;
            }
            return other.Value == Value &&
                   other.Span == Span &&
                   other.Kind == Kind;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ Span.GetHashCode() ^ Kind.GetHashCode();
        }

        public bool IsTrivia()
        {
            return Catagory == TokenCatagory.WhiteSpace || Catagory == TokenCatagory.Comment;
        }

        private TokenCatagory GetTokenCatagory()
        {
            switch (Kind)
            {
                case TokenKind.Arrow:
                case TokenKind.FatArrow:
                case TokenKind.Colon:
                case TokenKind.Semicolon:
                case TokenKind.Comma:
                case TokenKind.Dot:
                    return TokenCatagory.Punctuation;

                case TokenKind.Equal:
                case TokenKind.NotEqual:
                case TokenKind.Not:
                case TokenKind.LessThan:
                case TokenKind.LessThanOrEqual:
                case TokenKind.GreaterThan:
                case TokenKind.GreaterThanOrEqual:
                case TokenKind.Minus:
                case TokenKind.MinusEqual:
                case TokenKind.MinusMinus:
                case TokenKind.Mod:
                case TokenKind.ModEqual:
                case TokenKind.Mul:
                case TokenKind.MulEqual:
                case TokenKind.Plus:
                case TokenKind.PlusEqual:
                case TokenKind.PlusPlus:
                case TokenKind.Question:
                case TokenKind.DoubleQuestion:
                case TokenKind.DivEqual:
                case TokenKind.Div:
                case TokenKind.BooleanOr:
                case TokenKind.BooleanAnd:
                case TokenKind.BitwiseXorEqual:
                case TokenKind.BitwiseXor:
                case TokenKind.BitwiseOrEqual:
                case TokenKind.BitwiseOr:
                case TokenKind.BitwiseAndEqual:
                case TokenKind.BitwiseAnd:
                case TokenKind.BitShiftLeft:
                case TokenKind.BitShiftRight:
                case TokenKind.Assignment:
                    return TokenCatagory.Operator;

                case TokenKind.BlockComment:
                case TokenKind.LineComment:
                    return TokenCatagory.Comment;

                case TokenKind.NewLine:
                case TokenKind.WhiteSpace:
                    return TokenCatagory.WhiteSpace;

                case TokenKind.LeftBrace:
                case TokenKind.LeftBracket:
                case TokenKind.LeftParenthesis:
                case TokenKind.RightBrace:
                case TokenKind.RightBracket:
                case TokenKind.RightParenthesis:
                    return TokenCatagory.Grouping;

                case TokenKind.Identifier:
                case TokenKind.Keyword:
                    return TokenCatagory.Identifier;

                case TokenKind.StringLiteral:
                case TokenKind.IntegerLiteral:
                case TokenKind.FloatLiteral:
                    return TokenCatagory.Constant;

                case TokenKind.Error:
                    return TokenCatagory.Invalid;

                default: return TokenCatagory.Unknown;
            }
        }
    }
}
