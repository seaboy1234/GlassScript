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
        public SyntaxNode ParseStatement()
        {
            SyntaxNode value = null;
            if (_current == TokenKind.Keyword)
            {
                switch (_current.Value)
                {
                    case "true":
                    case "false":
                        value = ParseExpression();
                        break;

                    case "if":
                        value = ParseIfStatement();
                        break;

                    case "do":
                        value = ParseDoWhileStatement();
                        break;

                    case "while":
                        value = ParseWhileStatement();
                        break;

                    case "for":
                        value = ParseForStatement();
                        break;

                    case "switch":
                        value = ParseSwitchStatement();
                        break;

                    case "return":
                        value = ParseReturnStatement();
                        break;

                    case "var":
                        value = ParseVariableDeclaration();
                        break;

                    default:
                        throw UnexpectedToken("if, do, while, or switch");
                }
            }
            else if (_current == TokenKind.Semicolon)
            {
                var token = TakeSemicolon();
                AddError(Severity.Warning, "Possibly mistaken empty statement.", token.Span);
                return new EmptyStatement(CreateSpan(token));
            }
            else
            {
                MakeStatement(() =>
                {
                    value = ParseExpression();
                });
                return value;
            }
            if (_last != TokenKind.RightBracket)
            {
                TakeSemicolon();
            }
            return value;
        }

        private CaseStatement ParseCaseStatement()
        {
            List<Expression> conditions = new List<Expression>();
            List<SyntaxNode> contents = new List<SyntaxNode>();
            var start = _current;

            while (_current == "case" || _current == "default")
            {
                if (_current == "default")
                {
                    var word = Take();
                    Take(TokenKind.Colon);
                    var span = CreateSpan(word);
                    var condition = new BinaryExpression(span,
                                                             new ConstantExpression(span, "true", ConstantKind.Boolean),
                                                             new ConstantExpression(span, "true", ConstantKind.Boolean),
                                                         BinaryOperator.Equal);
                    conditions.Add(condition);
                }
                else
                {
                    Take();
                    var condition = ParseExpression();
                    Take(TokenKind.Colon);
                    conditions.Add(condition);
                }
            }

            while (_current != "case" && _current != TokenKind.RightBracket)
            {
                contents.Add(ParseStatement());
            }

            return new CaseStatement(CreateSpan(start), conditions, contents);
        }

        private WhileStatement ParseDoWhileStatement()
        {
            var start = TakeKeyword("do");
            var body = ParseStatementOrScope();

            TakeKeyword("while");
            var predicate = ParsePredicate();

            return new WhileStatement(CreateSpan(start), true, predicate, body);
        }

        private ElseStatement ParseElseStatement()
        {
            var start = TakeKeyword("else");

            var body = ParseStatementOrScope();
            return new ElseStatement(CreateSpan(start), body);
        }

        private BlockStatement ParseExpressionOrScope()
        {
            if (_current == TokenKind.LeftBracket)
            {
                return ParseScope();
            }
            else
            {
                var start = _current;
                var expr = ParseExpression();
                return new BlockStatement(CreateSpan(start), new[] { expr });
            }
        }

        private ForStatement ParseForStatement()
        {
            var start = TakeKeyword("for");
            SyntaxNode initialization = null;
            Expression condition = null;
            Expression increment = null;
            MakeBlock(() =>
            {
                if (_current == "var")
                {
                    initialization = ParseVariableDeclaration();
                }
                else if (_current == TokenKind.Semicolon)
                {
                    initialization = new EmptyStatement(_current.Span);
                }
                else
                {
                    initialization = ParseExpression();
                }
                TakeSemicolon();

                condition = ParseLogicalExpression();
                TakeSemicolon();

                increment = ParseExpression();
            }, TokenKind.LeftParenthesis, TokenKind.RightParenthesis);

            var block = ParseStatementOrScope();

            return new ForStatement(CreateSpan(start), initialization, condition, increment, block);
        }

        private IfStatement ParseIfStatement()
        {
            var start = TakeKeyword("if");
            var predicate = ParsePredicate();
            var body = ParseStatementOrScope();

            ElseStatement elseStatement = null;
            if (_current == "else")
            {
                elseStatement = ParseElseStatement();
            }

            return new IfStatement(CreateSpan(start), predicate, body, elseStatement);
        }

        private ReturnStatement ParseReturnStatement()
        {
            var start = TakeKeyword("return");
            var value = ParseExpression();

            return new ReturnStatement(CreateSpan(start), value);
        }

        private BlockStatement ParseScope()
        {
            List<SyntaxNode> contents = new List<SyntaxNode>();
            var start = _current;
            MakeBlock(() =>
            {
                contents.Add(ParseStatement());
            });

            return new BlockStatement(CreateSpan(start), contents);
        }

        private BlockStatement ParseStatementOrScope()
        {
            if (_current == TokenKind.LeftBracket)
            {
                return ParseScope();
            }
            else
            {
                var statement = ParseStatement();
                return new BlockStatement(statement.Span, new[] { statement });
            }
        }

        private SwitchStatement ParseSwitchStatement()
        {
            List<CaseStatement> cases = new List<CaseStatement>();

            var start = TakeKeyword("switch");

            Expression expr;
            MakeBlock(() => expr = ParseExpression(), TokenKind.LeftParenthesis, TokenKind.RightParenthesis);
            MakeBlock(() =>
            {
                while (_current == "case" || _current == "default")
                {
                    cases.Add(ParseCaseStatement());
                }
            });

            return new SwitchStatement(CreateSpan(start), cases);
        }

        private WhileStatement ParseWhileStatement()
        {
            var start = TakeKeyword("while");

            Expression expr = ParsePredicate();

            var body = ParseStatementOrScope();

            return new WhileStatement(CreateSpan(start), false, expr, body);
        }
    }
}
