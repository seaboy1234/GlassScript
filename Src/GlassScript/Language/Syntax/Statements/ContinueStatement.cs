using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class ContinueStatement : EmptyStatement
    {
        public override SyntaxKind Kind => SyntaxKind.ContinueStatement;

        public ContinueStatement(SourceSpan span) : base(span)
        {
        }
    }
}
