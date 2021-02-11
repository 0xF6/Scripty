namespace Scripty
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BuiltinFunctions;
    using Expressions;
    using Interfaces;
    using Literals;
    using Objects;
    using Statements;

    public static class Evaluator
    {
        public static readonly ScriptyBoolean True = new() {Value = true};
        public static readonly ScriptyBoolean False = new() {Value = false};
        public static readonly ScriptyNull ScriptyNull = new();

        public static readonly Dictionary<string, ScriptyBuiltin> Builtins = new()
        {
            {"length", Length.Build()},
            {"wln", Puts.Build()},
            {"first", First.Build()},
            {"last", Last.Build()},
            {"push", Push.Build()},
            {"rest", Rest.Build()},
            {"parseInt", ParseInt.Build()},
            {"parseFloat", ParseFloat.Build()}
        };

        public static IObject Eval(INode node, ScriptyEnvironment env) =>
            node.GetType().Name switch
            {
                nameof(IntegerLiteral) => new ScriptyInteger {Value = ((IntegerLiteral) node).Value},
                nameof(FloatLiteral) => new ScriptyFloat {Value = ((FloatLiteral) node).Value},
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
                nameof(StringLiteral) => new ScriptyString {Value = ((StringLiteral) node).Value},
                nameof(ArrayLiteral) => HandleArrayLiteralEval((ArrayLiteral) node, env),
                nameof(IndexExpression) => HandleIndexExpressionEval((IndexExpression) node, env),
                nameof(HashLiteral) => EvalHashLiteral((HashLiteral) node, env),
                _ => ScriptyNull
            };

        private static IObject EvalHashLiteral(HashLiteral node, ScriptyEnvironment env)
        {
            var pairs = new Dictionary<HashKey, HashPair>();

            foreach (var (keyNode, valueNode) in node.Pairs)
            {
                var key = Eval(keyNode, env);
                if (IsError(key)) return key;

                if (key is not IHashable hashKey) return new ScriptyError(14, key, null, null);

                var value = Eval(valueNode, env);
                if (IsError(value)) return value;

                var hashed = hashKey.HashKey();

                pairs.Add(hashed, new HashPair {Key = key, Value = value});
            }

            return new ScriptyHash {Pairs = pairs};
        }

        private static IObject HandleIndexExpressionEval(IndexExpression node, ScriptyEnvironment env)
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
            if (left.Type() == ObjectType.StringObj && index.Type() == ObjectType.IntegerObj)
                return EvalStringIndexExpression(left, index);
            if (left.Type() == ObjectType.HashObj) return EvalHashIndexExpression(left, index);
            return new ScriptyError(9, left, null, null);
        }

        private static IObject EvalHashIndexExpression(IObject left, IObject index)
        {
            if (left is not ScriptyHash hashObject) return new ScriptyError(15, left, null, null);
            if (index is not IHashable key) return new ScriptyError(14, index, null, null);
            var hashKey = key.HashKey();
            var pairExists = hashObject.Pairs.TryGetValue(hashKey, out var pair);
            return !pairExists ? new ScriptyError(16, hashObject, null, index) : pair.Value;
        }

        private static IObject EvalStringIndexExpression(IObject left, IObject index)
        {
            var stringObj = (ScriptyString) left;
            var idx = ((ScriptyInteger) index).Value;
            var max = stringObj.Value.Length - 1;
            if (idx < 0 || idx > max)
                return new ScriptyError(10, stringObj, null, (ScriptyInteger) index);
            return (ScriptyString) stringObj.Value[(int) idx].ToString();
        }

        private static IObject EvalArrayIndexExpression(IObject left, IObject index)
        {
            var arrObj = (ScriptyArray) left;
            var idx = ((ScriptyInteger) index).Value;
            var max = arrObj.Elements.Count - 1;
            if (idx < 0 || idx > max)
                return new ScriptyError(10, arrObj, null, (ScriptyInteger) index);

            return arrObj.Elements[(int) idx];
        }

        private static IObject HandleArrayLiteralEval(ArrayLiteral node, ScriptyEnvironment env)
        {
            var elements = EvalExpressions(node.Elements, env);
            if (elements.Count == 1 && IsError(elements.First())) return elements.First();

            return new ScriptyArray {Elements = elements};
        }

        private static IObject HandleCallExpressionEval(CallExpression node, ScriptyEnvironment env)
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
                case nameof(ScriptyFunction):
                    if (!(fn is ScriptyFunction function)) return new ScriptyError(6, null, fn.Type(), null);
                    var extendedEnv = ExtendFunctionEnv(function, args);
                    var evaluated = Eval(function.Body, extendedEnv);
                    return UnwrapReturnValue(evaluated);
                case nameof(ScriptyBuiltin):
                    return !(fn is ScriptyBuiltin builtin)
                        ? new ScriptyError(6, null, fn.Type(), null)
                        : builtin.Fn(args);
                default:
                    Console.WriteLine(fn.GetType().Name);
                    return new ScriptyError(6, null, fn.Type(), null);
            }
        }

        private static ScriptyEnvironment ExtendFunctionEnv(ScriptyFunction scriptyFunction, List<IObject> args)
        {
            var env = new ScriptyEnvironment(scriptyFunction.Environment);
            for (var i = 0; i < scriptyFunction.Parameters.Count; i++)
            {
                var functionParam = scriptyFunction.Parameters[i];
                env.Set(functionParam.Value, args[i]);
            }

            return env;
        }

        private static IObject UnwrapReturnValue(IObject evaluated)
        {
            var returnValue = evaluated as ReturnValue;
            return returnValue ?? evaluated;
        }

        private static List<IObject> EvalExpressions(List<IExpression> nodeArguments, ScriptyEnvironment env)
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

        private static IObject HandleFunctionLiteralEval(FunctionLiteral node, ScriptyEnvironment env)
        {
            var parameters = node.Parameters;
            var body = node.Body;
            return new ScriptyFunction {Parameters = parameters, Environment = env, Body = body};
        }

        private static IObject EvalIdentifier(Identifier node, ScriptyEnvironment env)
        {
            var value = env.Get(node.Value);
            if (!(value is null)) return value;
            var builtin = Builtins.GetValueOrDefault(node.Value, null);
            if (!(builtin is null)) return builtin;
            return new ScriptyError(5, null, node.Value, null);
        }

        private static IObject HandleLetStatementCase(LetStatement node, ScriptyEnvironment env)
        {
            var value = Eval(node.Value, env);
            return IsError(value) ? value : env.Set(node.Name.Value, value);
        }

        private static IObject EvalBlockStatement(BlockStatement node, ScriptyEnvironment env)
        {
            IObject result = null;

            foreach (var nodeStatement in node.Statements)
            {
                result = Eval(nodeStatement, env);

                if (result is ScriptyNull) continue;
                if (result.Type() == ObjectType.ErrorObj || result.Type() == ObjectType.ReturnValueObj)
                    return result;
            }

            return result;
        }

        private static IObject EvalProgram(Code node, ScriptyEnvironment env)
        {
            IObject result = null;
            foreach (var nodeStatement in node.Statements)
            {
                result = Eval(nodeStatement, env);
                switch (result.GetType().Name)
                {
                    case nameof(ReturnValue):
                        return ((ReturnValue) result).Value;
                    case nameof(ScriptyError):
                        return result;
                }
            }

            return result;
        }

        private static IObject HandleReturnStatementEval(ReturnStatement node, ScriptyEnvironment env)
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

        private static IObject EvalIfExpression(IfExpression node, ScriptyEnvironment env)
        {
            var condition = Eval(node.Condition, env);
            if (IsError(condition)) return condition;
            if (IsTruthy(condition))
                return Eval(node.Consequence, env);

            return !(node.Alternative is null) ? Eval(node.Alternative, env) : ScriptyNull;
        }

        private static bool IsTruthy(IObject obj)
        {
            if (Equals(obj, ScriptyNull)) return false;
            if (Equals(obj, True)) return true;
            return !Equals(obj, False);
        }

        private static IObject HandleInfixExpression(INode node, ScriptyEnvironment env)
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
                return EvalIntAndFloatInfixExpression(infixNodeOperator, left, right);

            if (left.Type() == ObjectType.FloatObj && right.Type() == ObjectType.FloatObj)
                return EvalIntAndFloatInfixExpression(infixNodeOperator, left, right);

            if (left.Type() == ObjectType.FloatObj && right.Type() == ObjectType.IntegerObj ||
                left.Type() == ObjectType.IntegerObj && right.Type() == ObjectType.FloatObj)
                return EvalIntAndFloatInfixExpression(infixNodeOperator, left, right);


            if (left.Type() == ObjectType.StringObj && right.Type() == ObjectType.StringObj)
                return EvalStringInfixExpression(infixNodeOperator, left, right);

            if (left.Type() != right.Type())
                return new ScriptyError(3, left, infixNodeOperator, right);

            return infixNodeOperator switch
            {
                "==" => NativeBoolToBooleanObject(Equals(left, right)),
                "!=" => NativeBoolToBooleanObject(!Equals(left, right)),
                _ => new ScriptyError(2, left, infixNodeOperator, right)
            };
        }

        private static IObject EvalIntAndFloatInfixExpression(string infixNodeOperator, IObject left, IObject right)
        {
            var leftVal = left.Type() == ObjectType.IntegerObj
                ? ((ScriptyInteger) left).Value
                : ((ScriptyFloat) left).Value;
            var rightVal = right.Type() == ObjectType.IntegerObj
                ? ((ScriptyInteger) right).Value
                : ((ScriptyFloat) right).Value;

            var resultTypeInteger = left.Type() == ObjectType.IntegerObj && right.Type() == ObjectType.IntegerObj;

            return infixNodeOperator switch
            {
                "+" => resultTypeInteger ? (ScriptyInteger) (leftVal + rightVal) : (ScriptyFloat) (leftVal + rightVal),
                "-" => resultTypeInteger ? (ScriptyInteger) (leftVal - rightVal) : (ScriptyFloat) (leftVal - rightVal),
                "*" => resultTypeInteger ? (ScriptyInteger) (leftVal * rightVal) : (ScriptyFloat) (leftVal * rightVal),
                "/" => resultTypeInteger ? (ScriptyInteger) (leftVal / rightVal) : (ScriptyFloat) (leftVal / rightVal),
                "<" => NativeBoolToBooleanObject(leftVal < rightVal),
                ">" => NativeBoolToBooleanObject(leftVal > rightVal),
                "!=" => NativeBoolToBooleanObject(!leftVal.Equals(rightVal)),
                "==" => NativeBoolToBooleanObject(leftVal.Equals(rightVal)),
                _ => new ScriptyError(2, left, infixNodeOperator, right)
            };
        }

        private static IObject EvalStringInfixExpression(string infixNodeOperator, IObject left, IObject right)
        {
            if (infixNodeOperator != "+") return new ScriptyError(2, left, infixNodeOperator, right);

            var leftVal = ((ScriptyString) left).Value;
            var rightVal = ((ScriptyString) right).Value;

            return new ScriptyString {Value = $"{leftVal}{rightVal}"};
        }

        private static IObject HandlePrefixExpression(INode node, ScriptyEnvironment env)
        {
            var prefixedNode = (PrefixExpression) node;
            var right = Eval(prefixedNode.Right, env);
            if (IsError(right)) return right;
            return EvalPrefixExpression(prefixedNode.Operator, right);
        }

        private static IObject EvalPrefixExpression(string op, IObject right) =>
            op switch
            {
                "!" => EvalBangOperatorExpression(right),
                "-" => EvalMinusPrefixOperatorExpression(right),
                _ => new ScriptyError(4, null, op, right)
            };


        private static IObject EvalMinusPrefixOperatorExpression(IObject right)
        {
            if (right.Type() == ObjectType.IntegerObj)
                return new ScriptyInteger {Value = -((ScriptyInteger) right).Value};


            if (right.Type() == ObjectType.FloatObj)
                return new ScriptyFloat {Value = -((ScriptyFloat) right).Value};


            return new ScriptyError(4, null, "-", right);
        }

        private static IObject EvalBangOperatorExpression(IObject right)
        {
            switch (right.GetType().Name)
            {
                case nameof(ScriptyBoolean):
                    var boolRight = (ScriptyBoolean) right;
                    return boolRight.Value == false ? True : False;
                case nameof(ScriptyNull):
                    return True;
                default:
                    return False;
            }
        }

        private static ScriptyBoolean NativeBoolToBooleanObject(bool input) => input ? True : False;
    }
}