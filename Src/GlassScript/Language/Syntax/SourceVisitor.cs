using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language.Syntax
{
    public abstract class SourceVisitor
    {
        public void Visit(SyntaxNode node)
        {
            switch (node.Catagory)
            {
                case SyntaxCatagory.Document:
                    VisitDocument(node as SourceDocument);
                    break;

                case SyntaxCatagory.Expression:
                    VisitExpression(node as Expression);
                    break;

                case SyntaxCatagory.Statement:
                    VisitStatement(node as Statement);
                    break;

                case SyntaxCatagory.Declaration:
                    VisitDeclaration(node as Declaration);
                    break;
            }
        }

        protected abstract void VisitArithmetic(BinaryExpression expression);

        protected abstract void VisitArrayAccess(ArrayAccessExpression expression);

        protected abstract void VisitAssignment(BinaryExpression expression);

        protected void VisitBinary(BinaryExpression expression)
        {
            switch (expression.Operator)
            {
                case BinaryOperator.Add:
                case BinaryOperator.Div:
                case BinaryOperator.Mod:
                case BinaryOperator.Sub:
                case BinaryOperator.Mul:
                    VisitArithmetic(expression);
                    break;

                case BinaryOperator.AddAssign:
                case BinaryOperator.AndAssign:
                case BinaryOperator.DivAssign:
                case BinaryOperator.ModAssign:
                case BinaryOperator.MulAssign:
                case BinaryOperator.OrAssign:
                case BinaryOperator.SubAssign:
                case BinaryOperator.XorAssign:
                    VisitAssignment(expression);
                    break;

                case BinaryOperator.Equal:
                case BinaryOperator.GreaterThan:
                case BinaryOperator.GreaterThanOrEqual:
                case BinaryOperator.LessThanOrEqual:
                case BinaryOperator.LogicalAnd:
                case BinaryOperator.LogicalOr:
                case BinaryOperator.NotEqual:
                    VisitLogical(expression);
                    break;

                case BinaryOperator.BitwiseAnd:
                case BinaryOperator.BitwiseOr:
                case BinaryOperator.BitwiseXor:
                    VisitBitwise(expression);
                    break;
            }
        }

        protected abstract void VisitBitwise(BinaryExpression expression);

        protected abstract void VisitBlock(BlockStatement statement);

        protected abstract void VisitBreak(BreakStatement statement);

        protected abstract void VisitCase(CaseStatement statement);

        protected abstract void VisitClass(ClassDeclaration classDeclaration);

        protected abstract void VisitConstant(ConstantExpression expression);

        protected abstract void VisitConstructor(ConstructorDeclaration constructorDeclaration);

        protected abstract void VisitContinue(ContinueStatement statement);

        protected void VisitDeclaration(Declaration node)
        {
            switch (node.Kind)
            {
                case SyntaxKind.ClassDeclaration:
                    VisitClass(node as ClassDeclaration);
                    break;

                case SyntaxKind.FieldDeclaration:
                    VisitField(node as FieldDeclaration);
                    break;

                case SyntaxKind.ConstructorDeclaration:
                    VisitConstructor(node as ConstructorDeclaration);
                    break;

                case SyntaxKind.PropertyDeclaration:
                    VisitProperty(node as PropertyDeclaration);
                    break;

                case SyntaxKind.ParameterDeclaration:
                    VisitParameter(node as ParameterDeclaration);
                    break;

                case SyntaxKind.MethodDeclaration:
                    VisitMethod(node as MethodDeclaration);
                    break;

                case SyntaxKind.VariableDeclaration:
                    VisitVariable(node as VariableDeclaration);
                    break;
            }
        }

        protected void VisitDocument(SourceDocument sourceDocument)
        {
            foreach (var node in sourceDocument.Children)
            {
                Visit(node);
            }
        }

        protected abstract void VisitElse(ElseStatement statement);

        protected abstract void VisitEmpty(EmptyStatement statement);

        protected void VisitExpression(Expression expression)
        {
            switch (expression.Kind)
            {
                case SyntaxKind.ArrayAccessExpression:
                    VisitArrayAccess(expression as ArrayAccessExpression);
                    break;

                case SyntaxKind.BinaryExpression:
                    VisitBinary(expression as BinaryExpression);
                    break;

                case SyntaxKind.ConstantExpression:
                    VisitConstant(expression as ConstantExpression);
                    break;

                case SyntaxKind.IdentifierExpression:
                    VisitIdentifier(expression as IdentifierExpression);
                    break;

                case SyntaxKind.LambdaExpression:
                    VisitLambda(expression as LambdaExpression);
                    break;

                case SyntaxKind.MethodCallExpression:
                    VisitMethodCall(expression as MethodCallExpression);
                    break;

                case SyntaxKind.NewExpression:
                    VisitNew(expression as NewExpression);
                    break;

                case SyntaxKind.ReferenceExpression:
                    VisitReference(expression as ReferenceExpression);
                    break;

                case SyntaxKind.UnaryExpression:
                    VisitUnary(expression as UnaryExpression);
                    break;
            }
        }

        protected abstract void VisitField(FieldDeclaration fieldDeclaration);

        protected abstract void VisitFor(ForStatement statement);

        protected abstract void VisitIdentifier(IdentifierExpression expression);

        protected abstract void VisitIf(IfStatement statement);

        protected abstract void VisitLambda(LambdaExpression expression);

        protected abstract void VisitLogical(BinaryExpression expression);

        protected abstract void VisitMethod(MethodDeclaration methodDeclaration);

        protected abstract void VisitMethodCall(MethodCallExpression expression);

        protected abstract void VisitNew(NewExpression expression);

        protected abstract void VisitParameter(ParameterDeclaration parameterDeclaration);

        protected abstract void VisitProperty(PropertyDeclaration propertyDeclaration);

        protected abstract void VisitReference(ReferenceExpression expression);

        protected void VisitStatement(Statement statement)
        {
            switch (statement.Kind)
            {
                case SyntaxKind.BlockStatement:
                    VisitBlock(statement as BlockStatement);
                    break;

                case SyntaxKind.BreakStatement:
                    VisitBreak(statement as BreakStatement);
                    break;

                case SyntaxKind.CaseStatement:
                    VisitCase(statement as CaseStatement);
                    break;

                case SyntaxKind.ContinueStatement:
                    VisitContinue(statement as ContinueStatement);
                    break;

                case SyntaxKind.ElseStatement:
                    VisitElse(statement as ElseStatement);
                    break;

                case SyntaxKind.EmptyStatement:
                    VisitEmpty(statement as EmptyStatement);
                    break;

                case SyntaxKind.ForStatement:
                    VisitFor(statement as ForStatement);
                    break;

                case SyntaxKind.IfStatement:
                    VisitIf(statement as IfStatement);
                    break;

                case SyntaxKind.SwitchStatement:
                    VisitSwitch(statement as SwitchStatement);
                    break;

                case SyntaxKind.WhileStatement:
                    VisitWhile(statement as WhileStatement);
                    break;
            }
        }

        protected abstract void VisitSwitch(SwitchStatement statement);

        protected abstract void VisitUnary(UnaryExpression expression);

        protected abstract void VisitVariable(VariableDeclaration variableDeclaration);

        protected abstract void VisitWhile(WhileStatement statement);
    }
}
