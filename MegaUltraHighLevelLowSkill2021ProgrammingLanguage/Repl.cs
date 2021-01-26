using System;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage
{
    public class Repl
    {
        private const string PROMPT = "> ";

        public void Run()
        {
            while (true)
            {
                Console.Write(PROMPT);
                var input = Console.ReadLine();
                if (input is null)
                {
                    return;
                }

                var lexer = new Lexer(input);
                for (var tok = lexer.NextToken(); tok.Type != Token.EOF; tok = lexer.NextToken())
                {
                    Console.WriteLine($"{{ Type: {tok.Type}, Literal: {tok.Literal}}}");
                }
            }
        }
    }
}