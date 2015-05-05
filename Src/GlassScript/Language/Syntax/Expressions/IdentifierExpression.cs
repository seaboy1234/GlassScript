using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class IdentifierExpression : Expression
    {
        public string Identifier { get; }

        public override SyntaxKind Kind => SyntaxKind.IdentifierExpression;

        public IdentifierExpression(SourceSpan span, string identifier) : base(span)
        {
            Identifier = identifier;
        }
    }
}
