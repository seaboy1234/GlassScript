using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class MethodCallExpression : Expression
    {
        public IEnumerable<Expression> Arguments { get; }

        public override SyntaxKind Kind => SyntaxKind.MethodCallExpression;

        public Expression Reference { get; }

        public MethodCallExpression(SourceSpan span, Expression reference, IEnumerable<Expression> arguments)
            : base(span)
        {
            Reference = reference;
            Arguments = arguments;
        }
    }
}
