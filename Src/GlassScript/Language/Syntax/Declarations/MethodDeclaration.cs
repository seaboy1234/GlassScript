using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class MethodDeclaration : Declaration
    {
        public BlockStatement Body { get; }

        public override SyntaxKind Kind => SyntaxKind.MethodDeclaration;

        public IEnumerable<ParameterDeclaration> Parameters { get; }

        public string ReturnType { get; }

        public MethodDeclaration(SourceSpan span, string name, string returnType, IEnumerable<ParameterDeclaration> parameters, BlockStatement body) : base(span, name)
        {
            ReturnType = returnType;
            Parameters = parameters;
            Body = body;
        }
    }
}
