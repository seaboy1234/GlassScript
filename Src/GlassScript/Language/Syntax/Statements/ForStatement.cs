using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class ForStatement : Statement
    {
        public BlockStatement Body { get; }

        public Expression Condition { get; }

        public Expression Increment { get; }

        public SyntaxNode Initialization { get; }

        public override SyntaxKind Kind => SyntaxKind.ForStatement;

        public ForStatement(SourceSpan span, SyntaxNode initialization, Expression condition, Expression increment, BlockStatement body) : base(span)
        {
            Initialization = initialization;
            Condition = condition;
            Increment = increment;
            Body = body;
        }
    }
}
