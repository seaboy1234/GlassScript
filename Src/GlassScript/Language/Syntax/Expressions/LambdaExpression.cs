using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class LambdaExpression : Expression
    {
        public BlockStatement Body { get; }

        public override SyntaxKind Kind => SyntaxKind.LambdaExpression;

        public IEnumerable<ParameterDeclaration> Parameters { get; }

        public LambdaExpression(SourceSpan span, IEnumerable<ParameterDeclaration> parameters, BlockStatement body) : base(span)
        {
            Parameters = parameters;
            Body = body;
        }

        public MethodDeclaration ToMethodDeclaration(string name)
        {
            return new MethodDeclaration(Span, name, "Object", Parameters, Body);
        }
    }
}
