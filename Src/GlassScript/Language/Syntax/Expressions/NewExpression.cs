using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class NewExpression : Expression
    {
        public IEnumerable<Expression> Arguments { get; }

        public override SyntaxKind Kind => SyntaxKind.NewExpression;

        public Expression Reference { get; }

        public NewExpression(SourceSpan span, Expression reference, IEnumerable<Expression> arguments) : base(span)
        {
            Reference = reference;
            Arguments = arguments;
        }
    }
}
