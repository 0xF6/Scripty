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
                new OperatorTest {Input = "-a * b", Expected = "(-(a * b))"},
                new OperatorTest {Input = "!-a", Expected = "(!(-a))"},
                new OperatorTest {Input = "a + b + c", Expected = "((a + b) + c)"},
                new OperatorTest {Input = "a * b * c", Expected = "((a * b) * c)"},
                new OperatorTest {Input = "a + b - c", Expected = "((a + b) - c)"},
                new OperatorTest {Input = "a * b / c", Expected = "((a * b) / c)"},
                new OperatorTest {Input = "a + b / c", Expected = "(a + (b / c))"},
                new OperatorTest {Input = "a + b * c + d / e - f", Expected = "(((a + (b * c)) + (d / e)) - f)"},
                new OperatorTest {Input = "a > b == c < b", Expected = "((a > b) == (c < b))"},
                new OperatorTest {Input = "a + b; -c * c", Expected = "(a + b)(-(c * c))"},
                new OperatorTest {Input = "a < b != c > b", Expected = "((a < b) != (c > b))"},
                new OperatorTest
                {
                    Input = "3 + 4 * 5 == 3 * 1 + 4 * 5",
                    Expected = "((3 + (4 * 5)) == ((3 * 1) + (4 * 5)))"
                },
                new OperatorTest {Input = "true", Expected = "true"},
                new OperatorTest {Input = "false", Expected = "false"},
                new OperatorTest {Input = "3 > 5 == false", Expected = "((3 > 5) == false)"},
                new OperatorTest {Input = "3 < 5 == true", Expected = "((3 < 5) == true)"},
                new OperatorTest {Input = "!(true == true)", Expected = "(!(true == true))"},
                new OperatorTest {Input = "a + add(b * c) + d", Expected = "((a + add((b * c))) + d)"},
                new OperatorTest
                {
                    Input = "add(a, b, 1, 2 * 3, 4 + 5, add(6, 7 * 8))",
                    Expected = "add(a, b, 1, (2 * 3), (4 + 5), add(6, (7 * 8)))"
                },
                new OperatorTest
                {
                    Input = "add(a + b + c * d / f + g)",
                    Expected = "add((((a + b) + ((c * d) / f)) + g))"
                },
            };

            foreach (var operatorTest in tests)
            {
                var lexer = new Lexer(operatorTest.Input);
                var parser = new Parser(lexer);
                var program = parser.ParseCode();
                StaticTests.CheckParserErrors(parser);
                var actual = program.Str();
                Assert.AreEqual(operatorTest.Expected, actual);
            }
        }
    }
}