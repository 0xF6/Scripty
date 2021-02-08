namespace ScriptyTests
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Scripty;
    using Scripty.Literals;
    using Scripty.Objects;
    using Scripty.Statements;
    using Array = Scripty.Objects.Array;

    public class ArrayLiteralParsingTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ArrayLiteralParsingTest()
        {
            const string input = "[1, 2 * 2, 3 + 3]";

            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseCode();
            StaticTests.CheckParserErrors(parser);

            Assert.AreEqual(nameof(ExpressionStatement), program.Statements.First().GetType().Name);

            var stmt = (ExpressionStatement) program.Statements.First();

            Assert.AreEqual(nameof(ArrayLiteral), stmt.Expression.GetType().Name);

            var array = (ArrayLiteral) stmt.Expression;

            Assert.AreEqual(3, array.Elements.Count);

            StaticTests.TestIntegerLiteral(array.Elements.First(), 1);
            IntegerLiteral two = 2;
            IntegerLiteral three = 3;
            StaticTests.TestInfixExpression(array.Elements[1], two, "*", two);
            StaticTests.TestInfixExpression(array.Elements[2], three, "+", three);
        }

        [Test]
        public void ArrayObjectTest()
        {
            const string input = "[1, 2 * 2, 3 + 3]";
            var evaluated = StaticTests.TestEval(input);

            Assert.AreEqual(nameof(Array), evaluated.GetType().Name);

            var array = (Array) evaluated;

            Assert.AreEqual(3, array.Elements.Count);

            StaticTests.TestIntegerObject(array.Elements.First(), 1);
            StaticTests.TestIntegerObject(array.Elements[1], 4);
            StaticTests.TestIntegerObject(array.Elements[2], 6);
        }

        [Test]
        public void ArrayIndexExpressionTest()
        {
            var passingTests = new IntegerTests[]
            {
                new() {Input = "[1,2,3][0]", Expected = 1},
                new() {Input = "[1,2,3][1];", Expected = 2},
                new() {Input = "[1,2,3][2];", Expected = 3},
                new() {Input = "let i = 0; [1][i];", Expected = 1},
                new() {Input = "[1,2,3][1 + -1];", Expected = 1},
                new() {Input = "let arr = [1,2,3]; arr[2];", Expected = 3},
                new() {Input = "let arr = [1,2,3]; arr[0] + arr[1] + arr[2];", Expected = 6},
                new() {Input = "let arr = [1,2,3]; let i = arr[0]; arr[i]", Expected = 2}
            };
            var failingTests = new OperatorTest[]
            {
                new() {Input = "[1,2,3][3]", Expected = "[MUHL10] index 3 is out of range for [1, 2, 3]"},
                new() {Input = "[1,2,3][-1]", Expected = "[MUHL10] index -1 is out of range for [1, 2, 3]"}
            };

            foreach (var passingTest in passingTests)
                ArrayIndexExpressionIterationTest(passingTest.Input, passingTest.Expected);

            foreach (var failingTest in failingTests)
                ArrayIndexExpressionIterationTest(failingTest.Input, failingTest.Expected);
        }

        private void ArrayIndexExpressionIterationTest(string input, object expected)
        {
            var evaluated = StaticTests.TestEval(input);

            switch (expected.GetType().Name)
            {
                case "Int64":
                    var integer = (long) expected;
                    StaticTests.TestIntegerObject(evaluated, integer);
                    break;
                case "String":
                    var str = (string) expected;
                    var err = (Error) evaluated;
                    Assert.AreEqual(str, err.Message);
                    break;
                default:
                    Console.WriteLine(expected.GetType().Name);
                    break;
            }
        }
    }
}