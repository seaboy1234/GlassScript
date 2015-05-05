using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class PropertyDeclaration : Declaration
    {
        public MethodDeclaration GetMethod { get; }

        public override SyntaxKind Kind => SyntaxKind.PropertyDeclaration;

        public MethodDeclaration SetMethod { get; }

        public string Type { get; }

        public PropertyDeclaration(SourceSpan span, string name, string type, MethodDeclaration getMethod, MethodDeclaration setMethod) : base(span, name)
        {
            Type = type;
            GetMethod = getMethod;
            SetMethod = setMethod;
        }
    }
}
