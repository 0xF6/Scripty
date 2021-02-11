namespace ScriptyExecutable
{
    using System;
    using System.Linq;

    internal static class Program
    {
        private const string LangName = "Scripty";
        private const string Prompt = "> ";

        private static int Main(string[] args)
        {
            if (args.Length > 0)
            {
                return SourceReader.Run(args.First());
            }

            Console.WriteLine($"Welcome to {LangName}");
            return Repl.Run();
        }
    }
}