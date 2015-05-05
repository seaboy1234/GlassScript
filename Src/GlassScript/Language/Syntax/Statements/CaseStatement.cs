using System;
using System.Collections.Generic;

namespace GlassScript.Language.Syntax
{
    public class CaseStatement : Statement
    {
        public IEnumerable<SyntaxNode> Body { get; }

        public IEnumerable<Expression> Cases { get; }

        public override SyntaxKind Kind => SyntaxKind.CaseStatement;

        public CaseStatement(SourceSpan span, IEnumerable<Expression> cases, IEnumerable<SyntaxNode> body) : base(span)
        {
            Body = body;
            Cases = cases;
        }
    }
}
