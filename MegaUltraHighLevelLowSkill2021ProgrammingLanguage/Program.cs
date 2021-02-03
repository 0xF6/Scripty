using System;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"Welcome to {typeof(Program).Namespace}");
            Repl.Run();
        }
    }
}