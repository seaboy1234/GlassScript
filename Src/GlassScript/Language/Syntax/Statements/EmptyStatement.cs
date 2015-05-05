using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class EmptyStatement : Statement
    {
        public override SyntaxKind Kind => SyntaxKind.EmptyStatement;

        public EmptyStatement(SourceSpan span) : base(span)
        {
        }
    }
}
