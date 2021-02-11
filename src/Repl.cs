namespace Scripty
{
    using System;
    using System.Collections.Generic;
    using Objects;

    public static class Repl
    {
        private const string Prompt = "> ";

        public static void Run(string std)
        {
            var env = new ScriptyEnvironment();
            while (true)
            {
                Console.Write(Prompt);
                var input = Console.ReadLine();
                if (input is null) return;
                input = std + input;
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