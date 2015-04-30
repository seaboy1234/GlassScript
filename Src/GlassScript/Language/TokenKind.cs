using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language
{
    public enum TokenKind
    {
        EndOfFile,
        Error,

        #region WhiteSpace

        WhiteSpace,
        NewLine,

        #endregion WhiteSpace

        #region Comments

        LineComment,
        BlockComment,

        #endregion Comments

        #region Constants

        IntegerLiteral,
        StringLiteral,
        FloatLiteral,

        #endregion Constants

        #region Identifiers

        Identifier,
        Keyword,

        #endregion Identifiers

        #region Groupings

        LeftBracket, // {
        RightBracket, // }
        RightBrace, // ]
        LeftBrace, // [
        LeftParenthesis, // (
        RightParenthesis, // )

        #endregion Groupings

        #region Operators

        GreaterThanOrEqual, // >=
        GreaterThan, // >

        LessThan, // <
        LessThanOrEqual, // <=

        PlusEqual, // +=
        PlusPlus, // ++
        Plus, // +

        MinusEqual, // -=
        MinusMinus, // --
        Minus, // -

        Assignment, // =

        Not, // !
        NotEqual, // !=

        Mul, // *
        MulEqual, // *=

        Div, // /
        DivEqual, // /=

        BooleanAnd, // &&
        BooleanOr, // ||

        BitwiseAnd, // &
        BitwiseOr, // |

        BitwiseAndEqual, // &=
        BitwiseOrEqual, // |=

        ModEqual, // %=
        Mod, // %

        BitwiseXorEqual, // ^=
        BitwiseXor, // ^

        DoubleQuestion, // ??
        Question, // ?

        Equal, // ==

        BitShiftLeft, // <<
        BitShiftRight, // >>

        #endregion Operators

        #region Punctuation

        Dot,
        Comma,
        Semicolon,
        Colon,
        Arrow, // ->
        FatArrow, // =>

        #endregion Punctuation
    }
}
