namespace ScriptyExecutable
{
    using System.IO;

    public static class SourceReader
    {
        public static int Run(string path)
        {
            var source = File.ReadAllText(path);
            // Parse File Content;
            return 0;
        }
    }
}