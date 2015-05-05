using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class ConstructorDeclaration : Declaration
    {
        public BlockStatement Body { get; }

        public override SyntaxKind Kind => SyntaxKind.ConstructorDeclaration;

        public IEnumerable<ParameterDeclaration> Parameters { get; }

        public ConstructorDeclaration(SourceSpan span, string name, IEnumerable<ParameterDeclaration> parameters, BlockStatement body) : base(span, name)
        {
            Body = body;
            Parameters = parameters;
        }
    }
}
