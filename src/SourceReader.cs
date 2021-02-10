namespace Scripty
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Objects;

    public static class SourceReader
    {
        public static int Run(string path, string std)
        {
            var source = std + File.ReadAllText(path);

            if (string.IsNullOrEmpty(source) || string.IsNullOrWhiteSpace(source))
                return 1;

            var env = new ScriptyEnvironment();
            var lexer = new Lexer(source);
            var parser = new Parser(lexer);

            var program = parser.ParseCode();
            if (parser.Errors.Count != 0)
            {
                PrintParserErrors(parser.Errors);
                return 1;
            }

            var evaluated = Evaluator.Eval(program, env);
            if (!(evaluated is null)) Console.WriteLine(evaluated.Inspect());
            return 0;
        }

        private static void PrintParserErrors(List<string> errors)
        {
            foreach (var error in errors)
                Console.WriteLine(error);
        }
    }
}