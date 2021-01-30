using System;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Welcome to {typeof(Program).Namespace}");
            new Repl().Run();
        }
    }
}