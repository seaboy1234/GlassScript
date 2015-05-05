using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class IfStatement : Statement
    {
        public BlockStatement Body { get; }

        public ElseStatement ElseStatement { get; }

        public override SyntaxKind Kind => SyntaxKind.IfStatement;

        public Expression Predicate { get; }

        public IfStatement(SourceSpan span, Expression predicate, BlockStatement body, ElseStatement elseStatement)
            : base(span)
        {
            Predicate = predicate;
            Body = body;
            ElseStatement = elseStatement;
        }
    }
}
