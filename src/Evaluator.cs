using System;
using System.Collections.Generic;
using System.Linq;
using Scripty.BuiltinFunctions;
using Scripty.Expressions;
using Scripty.Interfaces;
using Scripty.Literals;
using Scripty.Objects;
using Scripty.Statements;
using Array = Scripty.Objects.Array;
using Boolean = Scripty.Objects.Boolean;
using Environment = Scripty.Objects.Environment;
using String = Scripty.Objects.String;

namespace Scripty
{
    public static class Evaluator
    {
        public static readonly Boolean True = new() {Value = true};
        public static readonly Boolean False = new() {Value = false};
        public static readonly Null Null = new();

        public static readonly Dictionary<string, Builtin> Builtins = new()
        {
            {"length", Length.Build()},
            {"wln", Puts.Build()}
        };

        public static IObject Eval(INode node, Environment env)
        {
            return node.GetType().Name switch
            {
                nameof(IntegerLiteral) => new Integer {Value = ((IntegerLiteral) node).Value},
                nameof(ExpressionStatement) => Eval(((ExpressionStatement) node).Expression, env),
                nameof(Code) => EvalProgram((Code) node, env),
                nameof(BooleanLiteral) => NativeBoolToBooleanObject(((BooleanLiteral) node).Value),
                nameof(PrefixExpression) => HandlePrefixExpression(node, env),
                nameof(InfixExpression) => HandleInfixExpression(node, env),
                nameof(BlockStatement) => EvalBlockStatement((BlockStatement) node, env),
                nameof(IfExpression) => EvalIfExpression((IfExpression) node, env),
                nameof(ReturnStatement) => HandleReturnStatementEval((ReturnStatement) node, env),
                nameof(LetStatement) => HandleLetStatementCase((LetStatement) node, env),
                nameof(Identifier) => EvalIdentifier((Identifier) node, env),
                nameof(FunctionLiteral) => HandleFunctionLiteralEval((FunctionLiteral) node, env),
                nameof(CallExpression) => HandleCallExpressionEval((CallExpression) node, env),
                nameof(StringLiteral) => new String {Value = ((StringLiteral) node).Value},
                nameof(ArrayLiteral) => HandleArrayLiteralEval((ArrayLiteral) node, env),
                nameof(IndexExpression) => HandleIndexExpressionEval((IndexExpression) node, env),
                _ => Null
            };
        }

        private static IObject HandleIndexExpressionEval(IndexExpression node, Environment env)
        {
            var left = Eval(node.Left, env);
            if (IsError(left)) return left;

            var index = Eval(node.Index, env);
            if (IsError(index)) return index;

            return EvalIndexExpression(left, index);
        }

        private static IObject EvalIndexExpression(IObject left, IObject index)
        {
            if (left.Type() == ObjectType.ArrayObj && index.Type() == ObjectType.IntegerObj)
                return EvalArrayIndexExpression(left, index);
            return new Error(9, left, null, null);
        }

        private static IObject EvalArrayIndexExpression(IObject left, IObject index)
        {
            var arrObj = (Array) left;
            var idx = ((Integer) index).Value;
            var max = arrObj.Elements.Count - 1;
            if (idx < 0 || idx > max)
                return new Error(10, arrObj, null, (Integer) index);

            return arrObj.Elements[(int) idx];
        }

        private static IObject HandleArrayLiteralEval(ArrayLiteral node, Environment env)
        {
            var elements = EvalExpressions(node.Elements, env);
            if (elements.Count == 1 && IsError(elements.First())) return elements.First();

            return new Array {Elements = elements};
        }

        private static IObject HandleCallExpressionEval(CallExpression node, Environment env)
        {
            var function = Eval(node.Function, env);
            if (IsError(function)) return function;
            var args = EvalExpressions(node.Arguments, env);
            if (args.Count == 1 && IsError(args.First())) return args.First();
            return ApplyFunction(function, args);
        }

        private static IObject ApplyFunction(IObject fn, List<IObject> args)
        {
            switch (fn.GetType().Name)
            {
                case nameof(Function):
                    if (!(fn is Function function)) return new Error(6, null, fn.Type(), null);
                    var extendedEnv = ExtendFunctionEnv(function, args);
                    var evaluated = Eval(function.Body, extendedEnv);
                    return UnwrapReturnValue(evaluated);
                case nameof(Builtin):
                    return !(fn is Builtin builtin) ? new Error(6, null, fn.Type(), null) : builtin.Fn(args);
                default:
                    Console.WriteLine(fn.GetType().Name);
                    return new Error(6, null, fn.Type(), null);
            }
        }

        private static Environment ExtendFunctionEnv(Function function, List<IObject> args)
        {
            var env = new Environment(function.Environment);
            for (var i = 0; i < function.Parameters.Count; i++)
            {
                var functionParam = function.Parameters[i];
                env.Set(functionParam.Value, args[i]);
            }

            return env;
        }

        private static IObject UnwrapReturnValue(IObject evaluated)
        {
            var returnValue = evaluated as ReturnValue;
            return returnValue ?? evaluated;
        }

        private static List<IObject> EvalExpressions(List<IExpression> nodeArguments, Environment env)
        {
            var result = new List<IObject>();
            foreach (var evaluated in nodeArguments.Select(nodeArgument => Eval(nodeArgument, env)))
            {
                if (IsError(evaluated))
                {
                    result.Add(evaluated);
                    return result;
                }

                result.Add(evaluated);
            }

            return result;
        }

        private static IObject HandleFunctionLiteralEval(FunctionLiteral node, Environment env)
        {
            var parameters = node.Parameters;
            var body = node.Body;
            return new Function {Parameters = parameters, Environment = env, Body = body};
        }

        private static IObject EvalIdentifier(Identifier node, Environment env)
        {
            var value = env.Get(node.Value);
            if (!(value is null)) return value;
            var builtin = Builtins.GetValueOrDefault(node.Value, null);
            if (!(builtin is null)) return builtin;
            return new Error(5, null, node.Value, null);
        }

        private static IObject HandleLetStatementCase(LetStatement node, Environment env)
        {
            var value = Eval(node.Value, env);
            return IsError(value) ? value : env.Set(node.Name.Value, value);
        }

        private static IObject EvalBlockStatement(BlockStatement node, Environment env)
        {
            IObject result = null;

            foreach (var nodeStatement in node.Statements)
            {
                result = Eval(nodeStatement, env);

                if (result is Null) continue;
                if (result.Type() == ObjectType.ErrorObj || result.Type() == ObjectType.ReturnValueObj)
                    return result;
            }

            return result;
        }

        private static IObject EvalProgram(Code node, Environment env)
        {
            IObject result = null;
            foreach (var nodeStatement in node.Statements)
            {
                result = Eval(nodeStatement, env);
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

        private static IObject HandleReturnStatementEval(ReturnStatement node, Environment env)
        {
            var value = Eval(node.ReturnValue, env);
            if (IsError(value)) return value;
            return new ReturnValue {Value = value};
        }

        private static bool IsError(IObject value)
        {
            if (value is null) return false;
            return value.Type() == ObjectType.ErrorObj;
        }

        private static IObject EvalIfExpression(IfExpression node, Environment env)
        {
            var condition = Eval(node.Condition, env);
            if (IsError(condition)) return condition;
            if (IsTruthy(condition))
                return Eval(node.Consequence, env);

            return !(node.Alternative is null) ? Eval(node.Alternative, env) : Null;
        }

        private static bool IsTruthy(IObject obj)
        {
            if (Equals(obj, Null)) return false;
            if (Equals(obj, True)) return true;
            return !Equals(obj, False);
        }

        private static IObject HandleInfixExpression(INode node, Environment env)
        {
            var infixNode = (InfixExpression) node;
            var left = Eval(infixNode.Left, env);
            if (IsError(left)) return left;
            var right = Eval(infixNode.Right, env);
            if (IsError(right)) return right;
            return EvalInfixExpression(infixNode.Operator, left, right);
        }

        private static IObject EvalInfixExpression(string infixNodeOperator, IObject left, IObject right)
        {
            if (left.Type() == ObjectType.IntegerObj && right.Type() == ObjectType.IntegerObj)
                return EvalIntegerInfixExpression(infixNodeOperator, (Integer) left, (Integer) right);

            if (left.Type() == ObjectType.StringObj && right.Type() == ObjectType.StringObj)
                return EvalStringInfixExpression(infixNodeOperator, left, right);

            if (left.Type() != right.Type())
                return new Error(3, left, infixNodeOperator, right);

            return infixNodeOperator switch
            {
                "==" => NativeBoolToBooleanObject(Equals(left, right)),
                "!=" => NativeBoolToBooleanObject(!Equals(left, right)),
                _ => new Error(2, left, infixNodeOperator, right)
            };
        }

        private static IObject EvalStringInfixExpression(string infixNodeOperator, IObject left, IObject right)
        {
            if (infixNodeOperator != "+") return new Error(2, left, infixNodeOperator, right);

            var leftVal = ((String) left).Value;
            var rightVal = ((String) right).Value;

            return new String {Value = $"{leftVal}{rightVal}"};
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
                _ => new Error(2, left, infixNodeOperator, right)
            };
        }

        private static IObject HandlePrefixExpression(INode node, Environment env)
        {
            var prefixedNode = (PrefixExpression) node;
            var right = Eval(prefixedNode.Right, env);
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

        private static Boolean NativeBoolToBooleanObject(bool input)
        {
            return input ? True : False;
        }
    }
}