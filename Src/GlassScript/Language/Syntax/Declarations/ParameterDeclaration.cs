using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class ParameterDeclaration : Declaration
    {
        public override SyntaxKind Kind => SyntaxKind.ParameterDeclaration;

        public string Type { get; }

        public ParameterDeclaration(SourceSpan span, string name, string type) : base(span, name)
        {
            Type = type;
        }
    }
}
