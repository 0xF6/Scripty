using System;

namespace Scripty
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