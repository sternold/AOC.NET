namespace AOC21.Shared
{
    public static class FileReader
    {
        public static IEnumerable<string> ReadAllLines(string filename)
        {
            var text = File.ReadAllText(filename);
            return text.Split(Environment.NewLine);
        }

        public static IEnumerable<int> ReadAllLinesAsInteger(string filename)
        {
            var text = File.ReadAllText(filename);
            return text.Split(Environment.NewLine).Select(l => int.Parse(l));
        }
    }
}
