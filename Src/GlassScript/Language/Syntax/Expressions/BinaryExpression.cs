using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public class BinaryExpression : Expression
    {
        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;

        public Expression Left { get; }

        public BinaryOperator Operator { get; }

        public Expression Right { get; }

        public BinaryExpression(SourceSpan span, Expression left, Expression right, BinaryOperator op) : base(span)
        {
            Left = left;
            Right = right;
            Operator = op;
        }
    }

    public enum BinaryOperator
    {
        #region Assignment

        Assign,
        AddAssign,
        SubAssign,
        MulAssign,
        DivAssign,
        ModAssign,
        AndAssign,
        XorAssign,
        OrAssign,

        #endregion Assignment

        #region Logical

        LogicalOr,
        LogicalAnd,

        #endregion Logical

        #region Equality

        Equal,
        NotEqual,

        #endregion Equality

        #region Relational

        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,

        #endregion Relational

        #region Bitwise

        BitwiseAnd,
        BitwiseOr,
        BitwiseXor,

        #endregion Bitwise

        #region Shift

        LeftShift,
        RightShift,

        #endregion Shift

        #region Additive

        Add,
        Sub,

        #endregion Additive

        #region Multiplicative

        Mul,
        Div,
        Mod,

        #endregion Multiplicative
    }
}
