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

        public string Contents { get; }

        public TokenKind Kind { get; }

        public SourceSpan Span { get; }

        public Token(TokenKind kind, string contents, SourceLocation start, SourceLocation end)
        {
            Kind = kind;
            Contents = contents;
            Span = new SourceSpan(start, end);

            _catagory = new Lazy<TokenCatagory>(GetTokenCatagory);
        }

        public static bool operator !=(Token left, string right)
        {
            return left?.Contents != right;
        }

        public static bool operator !=(string left, Token right)
        {
            return right?.Contents != left;
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
            return left?.Contents == right;
        }

        public static bool operator ==(string left, Token right)
        {
            return right?.Contents == left;
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
            return other.Contents == Contents &&
                   other.Span == Span &&
                   other.Kind == Kind;
        }

        public override int GetHashCode()
        {
            return Contents.GetHashCode() ^ Span.GetHashCode() ^ Kind.GetHashCode();
        }

        private TokenCatagory GetTokenCatagory()
        {
            switch (Kind)
            {
                default: return TokenCatagory.Unknown;
            }
        }
    }
}
