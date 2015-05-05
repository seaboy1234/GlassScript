using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassScript.Language.Syntax;

namespace GlassScript.Language.Parser
{
    public sealed partial class GlassScriptParser
    {
        private bool IsAdditiveOperator()
        {
            switch (_current.Kind)
            {
                case TokenKind.Plus:
                case TokenKind.Minus:
                    return true;

                default: return false;
            }
        }

        private bool IsAssignmentOperator()
        {
            switch (_current.Kind)
            {
                case TokenKind.Assignment:
                case TokenKind.PlusEqual:
                case TokenKind.MinusEqual:
                case TokenKind.MulEqual:
                case TokenKind.DivEqual:
                case TokenKind.ModEqual:
                case TokenKind.BitwiseAndEqual:
                case TokenKind.BitwiseOrEqual:
                case TokenKind.BitwiseXorEqual:
                    return true;

                default: return false;
            }
        }

        private bool IsBitwiseOperator()
        {
            switch (_current.Kind)
            {
                case TokenKind.BitwiseAnd:
                case TokenKind.BitwiseOr:
                case TokenKind.BitwiseXor:
                    return true;

                default: return false;
            }
        }

        private bool IsEqualityOperator()
        {
            switch (_current.Kind)
            {
                case TokenKind.Equal:
                case TokenKind.NotEqual:
                    return true;

                default: return false;
            }
        }

        private bool IsLogicalOperator()
        {
            switch (_current.Kind)
            {
                case TokenKind.BooleanOr:
                case TokenKind.BooleanAnd:
                    return true;

                default: return false;
            }
        }

        private bool IsMultiplicativeOperator()
        {
            switch (_current.Kind)
            {
                case TokenKind.Mul:
                case TokenKind.Div:
                case TokenKind.Mod:
                    return true;

                default: return false;
            }
        }

        private bool IsPrefixUnaryOperator()
        {
            switch (_current.Kind)
            {
                case TokenKind.PlusPlus:
                case TokenKind.MinusMinus:
                case TokenKind.Not:
                case TokenKind.Minus:
                    return true;

                default: return false;
            }
        }

        private bool IsRelationalOperator()
        {
            switch (_current.Kind)
            {
                case TokenKind.GreaterThan:
                case TokenKind.LessThan:
                case TokenKind.GreaterThanOrEqual:
                case TokenKind.LessThanOrEqual:
                    return true;

                default: return false;
            }
        }

        private bool IsShiftOperator()
        {
            switch (_current.Kind)
            {
                case TokenKind.BitShiftLeft:
                case TokenKind.BitShiftRight:
                    return true;

                default: return false;
            }
        }

        private bool IsSuffixUnaryOperator()
        {
            switch (_current.Kind)
            {
                case TokenKind.PlusPlus:
                case TokenKind.MinusMinus:
                    return true;

                default: return false;
            }
        }

        private Expression ParseAdditiveExpression()
        {
            Expression left = ParseMultiplicativeExpression();
            while (IsAdditiveOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseMultiplicativeExpression();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
            }
            return left;
        }

        private Expression ParseArrayAccessExpression(Expression hint)
        {
            List<Expression> arguments = new List<Expression>();

            MakeBlock(() =>
            {
                arguments.Add(ParseExpression());
                while (_current == TokenKind.Comma)
                {
                    Take(TokenKind.Comma);
                    arguments.Add(ParseExpression());
                }
            }, TokenKind.LeftBrace, TokenKind.RightBrace);

            var expr = new ArrayAccessExpression(CreateSpan(hint), hint, arguments);

            if (_current == TokenKind.Dot)
            {
                return ParseReferenceExpression(expr);
            }

            return expr;
        }

        private Expression ParseAssignmentExpression()
        {
            Expression left = ParseLogicalExpression();
            if (IsAssignmentOperator())
            {   // Assignment is right-associative.
                var op = ParseBinaryOperator();
                Expression right = ParseAssignmentExpression();

                return new BinaryExpression(CreateSpan(left), left, right, op);
            }
            return left;
        }

        private BinaryOperator ParseBinaryOperator()
        {
            var token = Take();
            switch (token.Kind)
            {
                case TokenKind.Plus: return BinaryOperator.Add;
                case TokenKind.Minus: return BinaryOperator.Sub;
                case TokenKind.Assignment: return BinaryOperator.Assign;
                case TokenKind.PlusEqual: return BinaryOperator.AddAssign;
                case TokenKind.MinusEqual: return BinaryOperator.SubAssign;
                case TokenKind.MulEqual: return BinaryOperator.MulAssign;
                case TokenKind.DivEqual: return BinaryOperator.DivAssign;
                case TokenKind.ModEqual: return BinaryOperator.ModAssign;
                case TokenKind.BitwiseAndEqual: return BinaryOperator.AndAssign;
                case TokenKind.BitwiseOrEqual: return BinaryOperator.OrAssign;
                case TokenKind.BitwiseXorEqual: return BinaryOperator.XorAssign;
                case TokenKind.BitwiseAnd: return BinaryOperator.BitwiseAnd;
                case TokenKind.BitwiseOr: return BinaryOperator.BitwiseOr;
                case TokenKind.BitwiseXor: return BinaryOperator.BitwiseXor;
                case TokenKind.Equal: return BinaryOperator.Equal;
                case TokenKind.NotEqual: return BinaryOperator.NotEqual;
                case TokenKind.BooleanOr: return BinaryOperator.LogicalOr;
                case TokenKind.BooleanAnd: return BinaryOperator.LogicalAnd;
                case TokenKind.Mul: return BinaryOperator.Mul;
                case TokenKind.Div: return BinaryOperator.Div;
                case TokenKind.Mod: return BinaryOperator.Div;
                case TokenKind.GreaterThan: return BinaryOperator.GreaterThan;
                case TokenKind.LessThan: return BinaryOperator.LessThan;
                case TokenKind.GreaterThanOrEqual: return BinaryOperator.GreaterThanOrEqual;
                case TokenKind.LessThanOrEqual: return BinaryOperator.LessThanOrEqual;
                case TokenKind.BitShiftLeft: return BinaryOperator.LeftShift;
                case TokenKind.BitShiftRight: return BinaryOperator.RightShift;
            }

            _index--;
            throw UnexpectedToken("Binary Operator");
        }

        private Expression ParseBitwiseExpression()
        {
            Expression left = ParseShiftExpression();

            while (IsBitwiseOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseShiftExpression();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
            }
            return left;
        }

        private ConstantExpression ParseConstantExpression()
        {
            ConstantKind kind;
            if (_current == "true" || _current == "false")
            {
                kind = ConstantKind.Boolean;
            }
            else if (_current == TokenKind.StringLiteral)
            {
                kind = ConstantKind.String;
            }
            else if (_current == TokenKind.IntegerLiteral)
            {
                kind = ConstantKind.Integer;
            }
            else if (_current == TokenKind.FloatLiteral)
            {
                kind = ConstantKind.Float;
            }
            else
            {
                throw UnexpectedToken("Constant");
            }

            var token = Take();

            return new ConstantExpression(token.Span, token.Value, kind);
        }

        private Expression ParseEqualityExpression()
        {
            Expression left = ParseRelationalExpression();
            while (IsEqualityOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseRelationalExpression();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
            }
            return left;
        }

        private Expression ParseExpression()
        {
            return ParseAssignmentExpression();
        }

        private Expression ParseIdentiferExpression()
        {
            var token = Take(TokenKind.Identifier);

            return new IdentifierExpression(token.Span, token.Value);
        }

        private LambdaExpression ParseLambdaExpression(SourceLocation start, IEnumerable<ParameterDeclaration> arguments)
        {
            Take(TokenKind.FatArrow);
            var body = ParseExpressionOrScope();

            return new LambdaExpression(CreateSpan(start), arguments, body);
        }

        private Expression ParseLogicalExpression()
        {
            Expression left = ParseEqualityExpression();
            while (IsLogicalOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseEqualityExpression();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
            }
            return left;
        }

        private Expression ParseMethodCallExpression()
        {
            return ParseIdentiferExpression();
        }

        private Expression ParseMethodCallExpression(Expression reference)
        {
            var arguments = new List<Expression>();
            MakeBlock(() =>
            {
                if (_current != TokenKind.RightParenthesis)
                {
                    arguments.Add(ParseExpression());
                    while (_current == TokenKind.Comma)
                    {
                        arguments.Add(ParseExpression());
                    }
                }
            }, TokenKind.LeftParenthesis, TokenKind.RightParenthesis);

            return new MethodCallExpression(CreateSpan(reference), reference, arguments);
        }

        private Expression ParseMultiplicativeExpression()
        {
            Expression left = ParseUnaryExpression();
            while (IsMultiplicativeOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseUnaryExpression();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
            }
            return left;
        }

        private Expression ParseNewExpression()
        {
            var start = TakeKeyword("new");
            List<Expression> references = new List<Expression>();
            List<Expression> arguments = new List<Expression>();

            Expression reference;

            references.Add(ParseIdentiferExpression());
            while (_current == TokenKind.Dot)
            {
                Take(TokenKind.Dot);
                references.Add(ParseIdentiferExpression());
            }

            if (references.Count == 1)
            {
                reference = references.First();
            }
            else
            {
                reference = new ReferenceExpression(CreateSpan(references.First()), references);
            }

            MakeBlock(() =>
            {
                if (_current != TokenKind.RightParenthesis)
                {
                    arguments.Add(ParseExpression());
                    while (_current == TokenKind.Comma)
                    {
                        arguments.Add(ParseExpression());
                    }
                }
            }, TokenKind.LeftParenthesis, TokenKind.RightParenthesis);

            var expr = new NewExpression(CreateSpan(start), reference, arguments);
            if (_current == TokenKind.Dot)
            {
                return ParseReferenceExpression(expr);
            }

            return expr;
        }

        private Expression ParseOverrideExpression()
        {
            var start = Take(TokenKind.LeftParenthesis).Span.Start;

            if (_current == TokenKind.RightParenthesis)
            {
                Take(TokenKind.RightParenthesis);
                return ParseLambdaExpression(start, new ParameterDeclaration[0]);
            }

            var expr = ParseExpression();

            Take(TokenKind.RightParenthesis);

            return expr;
        }

        private Expression ParsePredicate()
        {
            Expression expr = null;
            MakeBlock(() =>
            {
                expr = ParseLogicalExpression();
            }, TokenKind.LeftParenthesis, TokenKind.RightParenthesis);

            return expr;
        }

        private UnaryOperator ParsePrefixUnaryOperator()
        {
            var token = _current;
            Advance();
            switch (token.Kind)
            {
                case TokenKind.PlusPlus: return UnaryOperator.PreIncrement;
                case TokenKind.MinusMinus: return UnaryOperator.PreDecrement;
                case TokenKind.Not: return UnaryOperator.Not;
                case TokenKind.Minus: return UnaryOperator.Negation;
            }
            _index--;
            throw UnexpectedToken("Unary Operator");
        }

        private Expression ParsePrimaryExpression()
        {
            if (_current == TokenKind.Identifier)
            {
                if (_next == TokenKind.Dot)
                {
                    return ParseReferenceExpression();
                }
                return ParseIdentiferExpression();
            }
            else if (_next == TokenKind.LeftParenthesis)
            {
                return ParseMethodCallExpression();
            }
            else if (_current.Catagory == TokenCatagory.Constant || _current == "true" || _current == "false")
            {
                return ParseConstantExpression();
            }
            else if (_current == TokenKind.LeftParenthesis)
            {
                return ParseOverrideExpression();
            }
            else if (_current == "new")
            {
                return ParseNewExpression();
            }
            else
            {
                if (_current.Catagory == TokenCatagory.Operator)
                {
                    var token = _current;
                    Advance();
                    throw SyntaxError(Severity.Error, $"'{token.Value}' is an invalid expression term.", token.Span);
                }
                throw UnexpectedToken("Expression Term");
            }
        }

        private Expression ParseReferenceExpression(Expression hint)
        {
            var references = new List<Expression>();
            references.Add(hint);

            do
            {
                Take(TokenKind.Dot);
                if (_current == TokenKind.Identifier)
                {
                    var expr = ParseIdentiferExpression();
                    references.Add(expr);
                }

                if (_current == TokenKind.LeftParenthesis)
                {
                    var expr = new ReferenceExpression(CreateSpan(hint), references);
                    return ParseMethodCallExpression(expr);
                }
                else if (_current == TokenKind.LeftBrace)
                {
                    var expr = new ReferenceExpression(CreateSpan(hint), references);
                    return ParseArrayAccessExpression(expr);
                }
            } while (_current == TokenKind.Dot);

            return new ReferenceExpression(CreateSpan(hint), references);
        }

        private Expression ParseReferenceExpression()
        {
            var hint = ParseIdentiferExpression();
            return ParseReferenceExpression(hint);
        }

        private Expression ParseRelationalExpression()
        {
            Expression left = ParseBitwiseExpression();

            while (IsRelationalOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseBitwiseExpression();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
            }
            return left;
        }

        private Expression ParseShiftExpression()
        {
            Expression left = ParseAdditiveExpression();
            while (IsShiftOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseAdditiveExpression();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
            }
            return left;
        }

        private UnaryOperator ParseSuffixUnaryOperator()
        {
            var token = _current;
            Advance();
            switch (token.Kind)
            {
                case TokenKind.PlusPlus: return UnaryOperator.PostIncrement;
                case TokenKind.MinusMinus: return UnaryOperator.PostDecrement;
            }
            _index--;
            throw UnexpectedToken("Unary Operator");
        }

        private Expression ParseUnaryExpression()
        {
            UnaryOperator op = UnaryOperator.Default;
            SourceLocation? start = null;

            if (IsPrefixUnaryOperator())
            {
                start = _current.Span.Start;

                op = ParsePrefixUnaryOperator();
            }

            if (start == null)
            {
                start = _current.Span.Start;
            }
            var expression = ParsePrimaryExpression();

            if (IsSuffixUnaryOperator())
            {
                op = ParseSuffixUnaryOperator();
            }

            if (op != UnaryOperator.Default)
            {
                return new UnaryExpression(CreateSpan(start.Value), expression, op);
            }
            return expression;
        }
    }
}
