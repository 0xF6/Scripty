using System.Collections.Generic;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Literals;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage
{
    public static class Evaluator
    {
        public static readonly Boolean True = new() {Value = true};
        public static readonly Boolean False = new() {Value = false};
        public static readonly Null Null = new();

        public static IObject Eval(INode node)
        {
            return node.GetType().Name switch
            {
                nameof(IntegerLiteral) => new Integer {Value = ((IntegerLiteral) node).Value},
                nameof(ExpressionStatement) => Eval(((ExpressionStatement) node).Expression),
                nameof(Code) => EvalStatement(((Code) node).Statements),
                nameof(BooleanLiteral) => NativeBoolToBooleanObject(((BooleanLiteral) node).Value),
                nameof(PrefixExpression) => HandlePrefixExpression(node),
                nameof(InfixExpression) => HandleInfixExpression(node),
                nameof(BlockStatement) => EvalStatement(((BlockStatement) node).Statements),
                nameof(IfExpression) => EvalIfExpression((IfExpression) node),
                _ => Null
            };
        }

        private static IObject EvalIfExpression(IfExpression node)
        {
            var condition = Eval(node.Condition);

            if (IsTruthy(condition))
                return Eval(node.Consequence);

            return !(node.Alternative is null) ? Eval(node.Alternative) : Null;
        }

        private static bool IsTruthy(IObject obj)
        {
            if (Equals(obj, Null)) return false;
            if (Equals(obj, True)) return true;
            return !Equals(obj, False);
        }

        private static IObject HandleInfixExpression(INode node)
        {
            var infixNode = (InfixExpression) node;
            var left = Eval(infixNode.Left);
            var right = Eval(infixNode.Right);
            return EvalInfixExpression(infixNode.Operator, left, right);
        }

        private static IObject EvalInfixExpression(string infixNodeOperator, IObject left, IObject right)
        {
            if (left.Type() == ObjectType.INTEGER_OBJ && right.Type() == ObjectType.INTEGER_OBJ)
                return EvalIntegerInfixExpression(infixNodeOperator, (Integer) left, (Integer) right);

            return infixNodeOperator switch
            {
                "==" => NativeBoolToBooleanObject(Equals(left, right)),
                "!=" => NativeBoolToBooleanObject(!Equals(left, right)),
                _ => Null
            };
        }

        private static IObject EvalIntegerInfixExpression(string infixNodeOperator, Integer left, Integer right)
        {
            var leftVal = left.Value;
            var rightVal = right.Value;
            return infixNodeOperator switch
            {
                "+" => new Integer {Value = leftVal + rightVal},
                "-" => new Integer {Value = leftVal - rightVal},
                "*" => new Integer {Value = leftVal * rightVal},
                "/" => new Integer {Value = leftVal / rightVal},
                "<" => NativeBoolToBooleanObject(leftVal < rightVal),
                ">" => NativeBoolToBooleanObject(leftVal > rightVal),
                "!=" => NativeBoolToBooleanObject(leftVal != rightVal),
                "==" => NativeBoolToBooleanObject(leftVal == rightVal),
                _ => Null
            };
        }

        private static IObject HandlePrefixExpression(INode node)
        {
            var prefixedNode = (PrefixExpression) node;
            var right = Eval(prefixedNode.Right);
            return EvalPrefixExpression(prefixedNode.Operator, right);
        }

        private static IObject EvalPrefixExpression(string op, IObject right)
        {
            return op switch
            {
                "!" => EvalBangOperatorExpression(right),
                "-" => EvalMinusPrefixOperatorExpression(right),
                _ => Null
            };
        }

        private static IObject EvalMinusPrefixOperatorExpression(IObject right)
        {
            if (right.Type() != ObjectType.INTEGER_OBJ) return Null;
            var value = ((Integer) right).Value;
            return new Integer {Value = -value};
        }

        private static IObject EvalBangOperatorExpression(IObject right)
        {
            switch (right.GetType().Name)
            {
                case "Boolean":
                    var boolRight = (Boolean) right;
                    return boolRight.Value == false ? True : False;
                case "Null":
                    return True;
                default:
                    return False;
            }
        }

        private static IObject EvalStatement(List<IStatement> statements)
        {
            IObject result = null;
            foreach (var statement in statements) result = Eval(statement);

            return result;
        }

        private static Boolean NativeBoolToBooleanObject(bool input)
        {
            return input ? True : False;
        }
    }
}