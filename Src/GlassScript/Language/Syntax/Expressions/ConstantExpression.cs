using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class ConstantExpression : Expression
    {
        public ConstantKind ConstentKind { get; }

        public override SyntaxKind Kind => SyntaxKind.ConstantExpression;

        public string Value { get; }

        public ConstantExpression(SourceSpan span, string value, ConstantKind kind)
            : base(span)
        {
            Value = value;
            ConstentKind = kind;
        }
    }

    public enum ConstantKind
    {
        Invalid,
        Integer,
        Float,
        String,
        Boolean
    }
}
