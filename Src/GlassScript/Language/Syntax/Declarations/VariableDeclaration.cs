using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class VariableDeclaration : Declaration
    {
        public override SyntaxKind Kind => SyntaxKind.VariableDeclaration;

        public string Type { get; }

        public Expression Value { get; }

        public VariableDeclaration(SourceSpan span, string name, string type, Expression value) : base(span, name)
        {
            Type = type;
            Value = value;
        }
    }
}
