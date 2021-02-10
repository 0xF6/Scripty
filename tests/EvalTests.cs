namespace ScriptyTests
{
    using NUnit.Framework;
    using Scripty.Objects;

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
                new() {Input = "5", Expected = 5},
                new() {Input = "228322", Expected = 228322},
                new() {Input = "-322", Expected = -322},
                new() {Input = "-228", Expected = -228},
                new() {Input = "5 + 5 + 5 + 5 - 10", Expected = 10},
                new() {Input = "2 * 2 * 2 * 2 * 2", Expected = 32},
                new() {Input = "100 + -50", Expected = 50},
                new() {Input = "-50 + 100 + -50", Expected = 0},
                new() {Input = "50 / 2 * 2 + 10", Expected = 60},
                new() {Input = "20 + 2 * -10", Expected = 0},
                new() {Input = "2 * (5 + 10)", Expected = 30},
                new() {Input = "(5 + 10 * 2 + 15 / 3) * 2 + -10", Expected = 50}
            };

            foreach (var integerTest in tests)
            {
                var evaluated = StaticTests.TestEval(integerTest.Input);
                StaticTests.TestIntegerObject(evaluated, integerTest.Expected);
            }
        }

        [Test]
        public void BooleanExpressionEvalTest()
        {
            var tests = new BooleanTest[]
            {
                new() {Input = "true", Value = true},
                new() {Input = "false", Value = false},
                new() {Input = "1 < 2", Value = true},
                new() {Input = "1 > 2", Value = false},
                new() {Input = "1 < 1", Value = false},
                new() {Input = "1 > 1", Value = false},
                new() {Input = "1 == 1", Value = true},
                new() {Input = "1 != 1", Value = false},
                new() {Input = "1 == 2", Value = false},
                new() {Input = "1 != 2", Value = true},
                new() {Input = "true == true", Value = true},
                new() {Input = "false == false", Value = true},
                new() {Input = "true == false", Value = false},
                new() {Input = "true != false", Value = true},
                new() {Input = "false != true", Value = true},
                new() {Input = "(1 < 2) == true", Value = true},
                new() {Input = "(1 < 2) == false", Value = false},
                new() {Input = "(1 > 2) == true", Value = false},
                new() {Input = "(1 > 2) == false", Value = true}
            };

            foreach (var booleanTest in tests)
            {
                var evaluated = StaticTests.TestEval(booleanTest.Input);
                StaticTests.TestBooleanObject(evaluated, booleanTest.Value);
            }
        }

        [Test]
        public void BangOperatorTest()
        {
            var tests = new BooleanTest[]
            {
                new() {Input = "!true", Value = false},
                new() {Input = "!false", Value = true},
                new() {Input = "!5", Value = false},
                new() {Input = "!!5", Value = true},
                new() {Input = "!!true", Value = true},
                new() {Input = "!!false", Value = false}
            };

            foreach (var booleanTest in tests)
            {
                var evaluated = StaticTests.TestEval(booleanTest.Input);
                StaticTests.TestBooleanObject(evaluated, booleanTest.Value);
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
                var evaluated = StaticTests.TestEval(statementEvalTestCase.Input);
                StaticTests.TestIntegerObject(evaluated, statementEvalTestCase.Expected);
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
                var evaluated = StaticTests.TestEval(boolEvalTestCase.Input);
                if (boolEvalTestCase.Expected is null)
                    Assert.True(StaticTests.TestNullObject(evaluated),
                        $"evaluated object is not Null, got {evaluated}");
                else
                    StaticTests.TestIntegerObject(evaluated, boolEvalTestCase.Expected);
            }
        }


        [Test]
        public void StringEvalTest()
        {
            const string input = "\"Allo, da-da, Tyulen u apparata, da.\"";

            var evaluated = StaticTests.TestEval(input);

            Assert.AreEqual(nameof(ScriptyString), evaluated.GetType().Name);

            var str = (ScriptyString) evaluated;

            Assert.AreEqual(input.Replace("\"", "").Replace("'", ""), str.Value);
        }


        [Test]
        public void StringConcatinationEvalTest()
        {
            const string input = "\"Allo, da-da,\" + \" \" + \"Tyulen u apparata, da.\"";
            const string expected = "Allo, da-da, Tyulen u apparata, da.";

            var evaluated = StaticTests.TestEval(input);

            Assert.AreEqual(nameof(ScriptyString), evaluated.GetType().Name);

            var str = (ScriptyString) evaluated;

            Assert.AreEqual(expected, str.Value);
        }

        [Test]
        public void BuiltinFunctionsTest()
        {
            var workingCases = new IntegerTests[]
            {
                new() {Input = "length(\"\");", Expected = 0},
                new() {Input = "length('something');", Expected = 9},
                new() {Input = "length('hello world');", Expected = 11}
            };
            var casesWithErrors = new OperatorTest[]
            {
                new() {Input = "length(1);", Expected = "[MUHL8] invalid operation: {op} has no length."},
                new() {Input = "length('one', '2');", Expected = "[MUHL7] wrong number of arguments: expected 1, got 2"}
            };

            foreach (var lengthWorkingTest in workingCases)
                StaticTests.TestBuiltinFunction(lengthWorkingTest.Input, lengthWorkingTest.Expected);

            foreach (var casesWithError in casesWithErrors)
                StaticTests.TestBuiltinFunction(casesWithError.Input, casesWithError.Expected);
        }
    }
}