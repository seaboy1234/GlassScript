using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class SwitchStatement : Statement
    {
        public IEnumerable<CaseStatement> Cases { get; }

        public override SyntaxKind Kind => SyntaxKind.SwitchStatement;

        public SwitchStatement(SourceSpan span, IEnumerable<CaseStatement> cases) : base(span)
        {
            Cases = cases;
        }
    }
}
