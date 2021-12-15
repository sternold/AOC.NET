namespace AOC21.Shared
{
    public static class FileReader
    {
        public static IEnumerable<string> ReadAllLines(string filename)
        {
            var text = File.ReadAllText(filename);
            return text.Split(Environment.NewLine);
        }

        public static IEnumerable<IEnumerable<string>> ReadAllParagraphs(string filename)
        {
            var text = File.ReadAllText(filename);
            return text.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(paragraph => paragraph.Split(Environment.NewLine));
        }

        public static IEnumerable<int> ReadAllLinesAsInteger(string filename)
        {
            var text = File.ReadAllText(filename);
            return text.Split(Environment.NewLine).Select(l => int.Parse(l));
        }

        public static int[,] ReadAsGrid(string filename)
        {
            var text = File.ReadAllText(filename);
            var lines = text.Split(Environment.NewLine);
            return lines.Select((line, i) => (Value: line, Index: i))
                         .Aggregate(new int[lines.Count(), lines.First().Length], (agg, next)
                            => next.Value.Select((c, i) => (Value: c.ToString(), Index: i))
                                         .Aggregate(agg, (agg2, next2) =>
                                         {
                                             agg2[next.Index, next2.Index] = int.Parse(next2.Value);
                                             return agg2;
                                         })); ;
        }
    }
}
