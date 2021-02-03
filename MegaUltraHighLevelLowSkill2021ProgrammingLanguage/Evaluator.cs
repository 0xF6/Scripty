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

        public static IObject? Eval(INode node)
        {
            return node.GetType().Name switch
            {
                nameof(IntegerLiteral) => new Integer {Value = ((IntegerLiteral) node).Value},
                nameof(ExpressionStatement) => Eval(((ExpressionStatement) node).Expression),
                nameof(Code) => EvalProgram((Code) node),
                nameof(BooleanLiteral) => NativeBoolToBooleanObject(((BooleanLiteral) node).Value),
                nameof(PrefixExpression) => HandlePrefixExpression(node),
                nameof(InfixExpression) => HandleInfixExpression(node),
                nameof(BlockStatement) => EvalBlockStatement((BlockStatement) node),
                nameof(IfExpression) => EvalIfExpression((IfExpression) node),
                nameof(ReturnStatement) => HandleReturnStatementEval((ReturnStatement) node),
                _ => Null
            };
        }

        private static IObject EvalBlockStatement(BlockStatement node)
        {
            IObject? result = null;

            foreach (var nodeStatement in node.Statements)
            {
                result = Eval(nodeStatement);

                if (result is null) continue;
                if (result.Type() == ObjectType.ErrorObj || result.Type() == ObjectType.ReturnValueObj)
                    return result;
            }

            return result;
        }

        private static IObject EvalProgram(Code node)
        {
            IObject result = null;
            foreach (var nodeStatement in node.Statements)
            {
                result = Eval(nodeStatement);
                switch (result.GetType().Name)
                {
                    case nameof(ReturnValue):
                        return ((ReturnValue) result).Value;
                    case nameof(Error):
                        return result;
                }
            }

            return result;
        }

        private static IObject HandleReturnStatementEval(ReturnStatement node)
        {
            var value = Eval(node.ReturnValue);
            if (IsError(value)) return value;
            return new ReturnValue {Value = value};
        }

        private static bool IsError(IObject? value)
        {
            if (value is null) return false;
            return value.Type() == ObjectType.ErrorObj;
        }

        private static IObject EvalIfExpression(IfExpression node)
        {
            var condition = Eval(node.Condition);
            if (IsError(condition)) return condition;
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
            if (IsError(left)) return left;
            var right = Eval(infixNode.Right);
            if (IsError(right)) return right;
            return EvalInfixExpression(infixNode.Operator, left, right);
        }

        private static IObject EvalInfixExpression(string infixNodeOperator, IObject left, IObject right)
        {
            if (left.Type() == ObjectType.IntegerObj && right.Type() == ObjectType.IntegerObj)
                return EvalIntegerInfixExpression(infixNodeOperator, (Integer) left, (Integer) right);

            if (left.Type() != right.Type())
                return new Error(3, left, infixNodeOperator, right);

            return infixNodeOperator switch
            {
                "==" => NativeBoolToBooleanObject(Equals(left, right)),
                "!=" => NativeBoolToBooleanObject(!Equals(left, right)),
                _ => new Error(2, left, infixNodeOperator, right),
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
                _ => new Error(2, left, infixNodeOperator, right),
            };
        }

        private static IObject HandlePrefixExpression(INode node)
        {
            var prefixedNode = (PrefixExpression) node;
            var right = Eval(prefixedNode.Right);
            if (IsError(right)) return right;
            return EvalPrefixExpression(prefixedNode.Operator, right);
        }

        private static IObject EvalPrefixExpression(string op, IObject right)
        {
            return op switch
            {
                "!" => EvalBangOperatorExpression(right),
                "-" => EvalMinusPrefixOperatorExpression(right),
                _ => new Error(4, null, op, right)
            };
        }


        private static IObject EvalMinusPrefixOperatorExpression(IObject right)
        {
            if (right.Type() != ObjectType.IntegerObj) return new Error(4, null, "-", right);
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
            foreach (var statement in statements)
            {
                result = Eval(statement);
                if (result is null) continue;
                if (result.GetType().Name == nameof(ReturnValue))
                    return ((ReturnValue) result).Value;
            }

            return result;
        }

        private static Boolean NativeBoolToBooleanObject(bool input)
        {
            return input ? True : False;
        }
    }
}