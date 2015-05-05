using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class FieldDeclaration : Declaration
    {
        public Expression DefaultValue { get; }

        public override SyntaxKind Kind => SyntaxKind.FieldDeclaration;

        public string Type { get; }

        public FieldDeclaration(SourceSpan span, string name, string type, Expression value) : base(span, name)
        {
            Type = type;
            DefaultValue = value;
        }
    }
}
