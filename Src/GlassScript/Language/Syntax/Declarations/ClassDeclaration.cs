using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class ClassDeclaration : Declaration
    {
        public IEnumerable<ConstructorDeclaration> Constructors { get; }

        public IEnumerable<FieldDeclaration> Fields { get; }

        public override SyntaxKind Kind => SyntaxKind.ClassDeclaration;

        public IEnumerable<MethodDeclaration> Methods { get; }

        public IEnumerable<PropertyDeclaration> Properties { get; }

        public ClassDeclaration(SourceSpan span, string name, IEnumerable<ConstructorDeclaration> constructors,
                                IEnumerable<FieldDeclaration> fields,
                                IEnumerable<MethodDeclaration> methods,
                                IEnumerable<PropertyDeclaration> properties)
            : base(span, name)
        {
            Constructors = constructors;
            Fields = fields;
            Methods = methods;
            Properties = properties;
        }
    }
}
