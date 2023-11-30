namespace AOC22.Shared
{
    public static class FileReader
    {
        public static string ReadAllText(string filename)
        {
            var text = File.ReadAllText(filename);
            return text;
        }

        public static IEnumerable<string> ReadAllLines(string filename)
        {
            var text = File.ReadAllText(filename);
            return text.Split(Environment.NewLine);
        }
    }
}
