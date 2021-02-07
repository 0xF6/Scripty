using System.Linq;
using NUnit.Framework;
using Scripty;
using Scripty.Literals;
using Scripty.Objects;
using Scripty.Statements;

namespace ScriptyTests
{
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
    }
}