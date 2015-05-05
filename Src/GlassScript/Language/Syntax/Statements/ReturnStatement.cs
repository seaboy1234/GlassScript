using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class ReturnStatement : Statement
    {
        public override SyntaxKind Kind => SyntaxKind.ReturnStatement;

        public Expression Value { get; }

        public ReturnStatement(SourceSpan span, Expression value) : base(span)
        {
            Value = value;
        }
    }
}
