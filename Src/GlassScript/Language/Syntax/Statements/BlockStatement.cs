using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class BlockStatement : Statement
    {
        public IEnumerable<SyntaxNode> Contents { get; }

        public override SyntaxKind Kind => SyntaxKind.BlockStatement;

        public BlockStatement(SourceSpan span, IEnumerable<SyntaxNode> contents) : base(span)
        {
            Contents = contents;
        }
    }
}
