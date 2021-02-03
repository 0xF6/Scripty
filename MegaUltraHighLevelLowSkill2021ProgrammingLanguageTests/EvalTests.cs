using MegaUltraHighLevelLowSkill2021ProgrammingLanguage;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects;
using NUnit.Framework;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguageTests
{
    internal struct StatementEvalTestCase
    {
        public string Input { get; set; }
        public int? Expected { get; set; }
    }

    internal struct IntegerTests
    {
        public string Input { get; set; }
        public long expected { get; set; }
    }

    internal struct BooleanTests
    {
        public string Input { get; set; }
        public bool expected { get; set; }
    }

    public class EvalTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IntegerExpressionEvalTest()
        {
            var tests = new IntegerTests[]
            {
                new() {Input = "5", expected = 5},
                new() {Input = "228322", expected = 228322},
                new() {Input = "-322", expected = -322},
                new() {Input = "-228", expected = -228},
                new() {Input = "5 + 5 + 5 + 5 - 10", expected = 10},
                new() {Input = "2 * 2 * 2 * 2 * 2", expected = 32},
                new() {Input = "100 + -50", expected = 50},
                new() {Input = "-50 + 100 + -50", expected = 0},
                new() {Input = "50 / 2 * 2 + 10", expected = 60},
                new() {Input = "20 + 2 * -10", expected = 0},
                new() {Input = "2 * (5 + 10)", expected = 30},
                new() {Input = "(5 + 10 * 2 + 15 / 3) * 2 + -10", expected = 50}
            };

            foreach (var integerTest in tests)
            {
                var evaluated = TestEval(integerTest.Input);
                TestIntegerObject(evaluated, integerTest.expected);
            }
        }

        [Test]
        public void BooleanExpressionEvalTest()
        {
            var tests = new BooleanTests[]
            {
                new() {Input = "true", expected = true},
                new() {Input = "false", expected = false},
                new() {Input = "1 < 2", expected = true},
                new() {Input = "1 > 2", expected = false},
                new() {Input = "1 < 1", expected = false},
                new() {Input = "1 > 1", expected = false},
                new() {Input = "1 == 1", expected = true},
                new() {Input = "1 != 1", expected = false},
                new() {Input = "1 == 2", expected = false},
                new() {Input = "1 != 2", expected = true},
                new() {Input = "true == true", expected = true},
                new() {Input = "false == false", expected = true},
                new() {Input = "true == false", expected = false},
                new() {Input = "true != false", expected = true},
                new() {Input = "false != true", expected = true},
                new() {Input = "(1 < 2) == true", expected = true},
                new() {Input = "(1 < 2) == false", expected = false},
                new() {Input = "(1 > 2) == true", expected = false},
                new() {Input = "(1 > 2) == false", expected = true}
            };

            foreach (var booleanTest in tests)
            {
                var evaluated = TestEval(booleanTest.Input);
                TestBooleanObject(evaluated, booleanTest.expected);
            }
        }

        [Test]
        public void BangOperatorTest()
        {
            var tests = new BooleanTests[]
            {
                new() {Input = "!true", expected = false},
                new() {Input = "!false", expected = true},
                new() {Input = "!5", expected = false},
                new() {Input = "!!5", expected = true},
                new() {Input = "!!true", expected = true},
                new() {Input = "!!false", expected = false}
            };

            foreach (var booleanTest in tests)
            {
                var evaluated = TestEval(booleanTest.Input);
                TestBooleanObject(evaluated, booleanTest.expected);
            }
        }

        [Test]
        public void ReturnStatementTest()
        {
            var tests = new StatementEvalTestCase[]
            {
                new() {Input = "return 10;", Expected = 10},
                new() {Input = "return 10; 9;", Expected = 10},
                new() {Input = "return 2 * 5; 9;", Expected = 10},
                new() {Input = "9; return 2 * 5; 9;", Expected = 10},
                new()
                {
                    Input = "if (10 > 1) { if (15 > 1) { return 10; } return 1; }", Expected = 10
                }
            };

            foreach (var statementEvalTestCase in tests)
            {
                var evaluated = TestEval(statementEvalTestCase.Input);
                TestIntegerObject(evaluated, statementEvalTestCase.Expected);
            }
        }

        [Test]
        public void IfElseExpressionTest1()
        {
            var tests = new StatementEvalTestCase[]
            {
                new() {Input = "if (true) { 10 }", Expected = 10},
                new() {Input = "if (false) { 10 }", Expected = null},
                new() {Input = "if (1) { 10 }", Expected = 10},
                new() {Input = "if (1 < 2) { 10 }", Expected = 10},
                new() {Input = "if (1 > 2) { 10 }", Expected = null},
                new() {Input = "if (1 > 2) { 10 } else { 20 }", Expected = 20},
                new() {Input = "if (1 < 2) { 10 } else { 20 }", Expected = 10}
            };

            foreach (var boolEvalTestCase in tests)
            {
                var evaluated = TestEval(boolEvalTestCase.Input);
                if (boolEvalTestCase.Expected is null)
                    Assert.True(TestNullObject(evaluated), $"evaluated object is not Null, got {evaluated}");
                else
                    TestIntegerObject(evaluated, boolEvalTestCase.Expected);
            }
        }

        private static bool TestNullObject(IObject evaluated)
        {
            return Equals(evaluated, Evaluator.Null);
        }

        private static void TestBooleanObject(IObject obj, bool expected)
        {
            Assert.AreEqual("Boolean", obj.GetType().Name);

            var result = obj as Boolean;

            Assert.AreEqual(expected, result.Value);
        }

        private static void TestIntegerObject(IObject obj, long? expected)
        {
            Assert.AreEqual("Integer", obj.GetType().Name);

            var result = obj as Integer;

            Assert.AreEqual(expected, result.Value);
        }

        private static IObject TestEval(string input)
        {
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseCode();

            return Evaluator.Eval(program);
        }
    }
}