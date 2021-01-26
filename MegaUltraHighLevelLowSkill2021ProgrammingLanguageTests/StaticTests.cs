using System;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Literals;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements;
using NUnit.Framework;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguageTests
{
    public static class StaticTests
    {
        public static void CheckParserErrors(Parser parser)
        {
            var errors = parser.Errors;
            Assert.AreEqual(0, errors.Count);
        }

        public static void TestBooleanLiteral(IExpression exp, bool val)
        {
            Assert.AreEqual("BooleanLiteral", exp.GetType().Name);

            var boolLiteral = exp as BooleanLiteral;

            Assert.AreEqual(val, boolLiteral.Value);

            Assert.AreEqual($"{val}", boolLiteral.TokenLiteral());
        }

        public static void TestIntegerLiteral(IExpression exp, long val)
        {
            Assert.AreEqual("IntegerLiteral", exp.GetType().Name,
                $"expression right side is not f type 'IntegerLiteral', got={exp.GetType().Name}");

            var integerLiteral = exp as IntegerLiteral;

            Assert.AreEqual(integerLiteral.Value, val,
                $"integerLiteral value is not {val}, got={integerLiteral.Value}");

            Assert.AreEqual(integerLiteral.TokenLiteral(), $"{val}",
                $"integerLiterals token literal is not '{val}', got='{integerLiteral.TokenLiteral()}'");
        }

        public static void TestIdentifier(IExpression exp, string value)
        {
            Assert.AreEqual("Identifier", exp.GetType().Name);

            var ident = exp as Identifier;

            Assert.AreEqual(value, ident.Value);

            Assert.AreEqual(value, ident.TokenLiteral());
        }

        public static void TestLiteralExpression(IExpression exp, IExpression expected)
        {
            switch (expected.GetType().Name)
            {
                case "IntegerLiteral":
                    var integerExpectation = expected as IntegerLiteral;
                    TestIntegerLiteral(exp, integerExpectation.Value);
                    break;
                case "Identifier":
                    var identifierExpectation = expected as Identifier;
                    TestIdentifier(exp, identifierExpectation.Value);
                    break;
                case "BooleanLiteral":
                    var booleanExpectation = expected as BooleanLiteral;
                    TestBooleanLiteral(exp, booleanExpectation.Value);
                    break;
                default:
                    Console.Out.WriteLine(expected.GetType().Name);
                    break;
            }
        }

        public static void TestInfixExpression(IExpression exp, IExpression left, string op, IExpression right)
        {
            Assert.AreEqual(left.GetType().Name, right.GetType().Name);

            Assert.AreEqual("InfixExpression", exp.GetType().Name,
                $"expression not of type 'InfixExpression', got={exp.GetType().Name}");

            var opExp = exp as InfixExpression;

            StaticTests.TestLiteralExpression(opExp.Left, left);

            Assert.AreEqual(op, opExp.Operator,
                $"operator is not {op}, got={opExp.Operator}");


            StaticTests.TestLiteralExpression(opExp.Right, right);
        }

        public static void TestLetStatement(LetStatement statement, string name)
        {
            Assert.AreEqual(statement.TokenLiteral(), "let",
                $"statement token literal not 'let', got={statement.TokenLiteral()}");
            Assert.AreEqual(statement.GetType().Name, "LetStatement",
                $"statement is not of LetStatement type, got={statement.GetType().Name}");
            Assert.AreEqual(name, statement.Name.Value, $"statement name value not {name}, got={statement.Name.Value}");
            Assert.AreEqual(name, statement.Name.TokenLiteral(),
                $"statement name token literal not {name}, got={statement.Name.TokenLiteral()}");
        }
    }
}