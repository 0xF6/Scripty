namespace Scripty
{
    using System;
    using System.IO;
    using System.Linq;

    internal class Program
    {
        private static readonly string[] StdLoadOrder =
        {
            "map.scripty", "reduce.scripty", "sum.scripty"
        };

        private static int Main(string[] args)
        {
            var std = LoadStd();
            if (args.Length == 1)
            {
                var returnCode = SourceReader.Run(args.First(), std);
                Console.WriteLine($"program exited with code {returnCode}");
                return returnCode;
            }

            Console.WriteLine($"Welcome to {typeof(Program).Namespace}");
            Repl.Run(std);
            return 0;
        }

        private static string LoadStd()
        {
            var folder = $"{Environment.CurrentDirectory}/Std";

            return StdLoadOrder.Aggregate("", (current, path) => $"{current}{File.ReadAllText($"{folder}/{path}")}\n");
        }
    }
}