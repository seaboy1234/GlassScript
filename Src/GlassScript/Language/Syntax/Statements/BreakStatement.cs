using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class BreakStatement : EmptyStatement
    {
        public override SyntaxKind Kind => SyntaxKind.BreakStatement;

        public BreakStatement(SourceSpan span) : base(span)
        {
        }
    }
}
