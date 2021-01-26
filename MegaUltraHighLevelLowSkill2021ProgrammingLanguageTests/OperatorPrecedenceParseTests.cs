using MegaUltraHighLevelLowSkill2021ProgrammingLanguage;
using NUnit.Framework;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguageTests
{
    struct OperatorTest
    {
        public string Input { get; set; }
        public string Expected { get; set; }
    }

    public class OperatorPrecedenceParseTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void OperatorPrecedenceParseTest()
        {
            var tests = new OperatorTest[]
            {
                new OperatorTest {Input = "-a * b", Expected = "((-a) * b)"},
                new OperatorTest {Input = "!-a", Expected = "(!(-a))"},
                new OperatorTest {Input = "a + b + c", Expected = "((a + b) + c)"},
                new OperatorTest {Input = "a * b * c", Expected = "((a * b) * c)"},
                new OperatorTest {Input = "a + b - c", Expected = "((a + b) - c)"},
                new OperatorTest {Input = "a * b / c", Expected = "((a * b) / c)"},
                new OperatorTest {Input = "a + b / c", Expected = "(a + (b / c))"},
                new OperatorTest {Input = "a + b * c + d / e - f", Expected = "(((a + (b * c)) + (d / e)) - f)"},
                new OperatorTest {Input = "a > b == c < b", Expected = "((a > b) == (c < b))"},
                new OperatorTest {Input = "a + b; -c * c", Expected = "(a + b)((-c) * c)"},
                new OperatorTest {Input = "a < b != c > b", Expected = "((a < b) != (c > b))"},
                new OperatorTest
                    {Input = "3 + 4 * 5 == 3 * 1 + 4 * 5", Expected = "((3 + (4 * 5)) == ((3 * 1) + (4 * 5)))"},
            };

            foreach (var operatorTest in tests)
            {
                var lexer = new Lexer(operatorTest.Input);
                var parser = new Parser(lexer);
                var program = parser.ParseCode();
                CheckParserErrors(parser);
                var actual = program.Str();
                Assert.AreEqual(operatorTest.Expected, actual);
            }
        }

        private void CheckParserErrors(Parser parser)
        {
            var errors = parser.Errors;
            Assert.AreEqual(0, errors.Count);
        }
    }
}