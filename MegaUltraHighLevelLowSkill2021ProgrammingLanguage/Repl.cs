using System;
using System.Collections.Generic;
using Environment = MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects.Environment;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage
{
    public static class Repl
    {
        private const string Prompt = "> ";

        public static void Run()
        {
            var env = new Environment();
            while (true)
            {
                Console.Write(Prompt);
                var input = Console.ReadLine();
                if (input is null) return;

                var lexer = new Lexer(input);
                var parser = new Parser(lexer);

                var program = parser.ParseCode();
                if (parser.Errors.Count != 0)
                {
                    PrintParserErrors(parser.Errors);
                    continue;
                }

                var evaluated = Evaluator.Eval(program, env);
                if (!(evaluated is null)) Console.WriteLine(evaluated.Inspect());
            }
        }

        private static void PrintParserErrors(List<string> errors)
        {
            foreach (var error in errors)
                Console.WriteLine(error);
        }
    }
}