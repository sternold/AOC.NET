namespace AOC21
{
    public class Day08 : BaseDay
    {
        public enum Display
        {
            Top,
            TopLeft,
            TopRight,
            Middle,
            BottomLeft,
            BottomRight,
            Bottom
        }

        public readonly Dictionary<int, Display[]> Configurations = new()
        {
            { 0, new Display[] { Display.Top, Display.TopLeft, Display.TopRight, Display.BottomLeft, Display.BottomRight, Display.Bottom } },
            { 1, new Display[] { Display.TopRight, Display.BottomRight } },
            { 2, new Display[] { Display.Top, Display.TopRight, Display.Middle, Display.BottomLeft, Display.Bottom } },
            { 3, new Display[] { Display.Top, Display.TopLeft, Display.TopRight, Display.Middle, Display.BottomLeft, Display.BottomRight, Display.Bottom } },
            { 4, new Display[] { Display.TopLeft, Display.TopRight, Display.Middle, Display.BottomRight } },
            { 5, new Display[] { Display.Top, Display.TopLeft, Display.Middle, Display.BottomRight, Display.Bottom } },
            { 6, new Display[] { Display.Top, Display.TopLeft, Display.Middle, Display.BottomLeft, Display.BottomRight, Display.Bottom } },
            { 7, new Display[] { Display.Top, Display.TopRight, Display.BottomRight } },
            { 8, new Display[] { Display.Top, Display.TopLeft, Display.TopRight, Display.Middle, Display.BottomLeft, Display.BottomRight, Display.Bottom } },
            { 9, new Display[] { Display.Top, Display.TopLeft, Display.TopRight, Display.Middle, Display.BottomRight, Display.Bottom } },
        };

        public IEnumerable<(string[] Signals, string[] Output)> Input { get; }

        public Day08()
        {
            Input = FileReader.ReadAllLines(InputFilePath)
                .Select((line) => line.Split('|', StringSplitOptions.RemoveEmptyEntries)
                                      .Select((signals) => signals.Trim()
                                                                  .Split(' ', StringSplitOptions.RemoveEmptyEntries))
                                                                  .ToArray())
                .Select(arr => (arr[0], arr[1]));
        }

        public override ValueTask<string> Solve_1()
        {
            var oneLength = Configurations[1].Length;
            var fourLength = Configurations[4].Length;
            var sevenLength = Configurations[7].Length;
            var eightLength = Configurations[8].Length;
            return new(Input.Sum(display => display.Output.Count(num => num.Length == oneLength || num.Length == fourLength || num.Length == sevenLength || num.Length == eightLength)).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var oneLength = Configurations[1].Length;
            var fourLength = Configurations[4].Length;
            var sevenLength = Configurations[7].Length;
            var eightLength = Configurations[8].Length;
            var length235 = Configurations[2].Length;
            var length069 = Configurations[0].Length;
            return new(Input.Sum(display =>
            {
                Dictionary<int, string> dict = new();
                dict[1] = display.Signals.First(s => s.Length == oneLength);
                dict[4] = display.Signals.First(s => s.Length == fourLength);
                dict[7] = display.Signals.First(s => s.Length == sevenLength);
                dict[8] = display.Signals.First(s => s.Length == eightLength);
                dict[3] = display.Signals.Where(s => s.Length == length235).First(s => dict[7].All(c => s.Contains(c)));
                dict[5] = display.Signals.Where(s => s.Length == length235 && s != dict[3]).First(s => dict[4].Count(c => s.Contains(c)) == 3);
                dict[2] = display.Signals.First(s => s.Length == length235 && s != dict[3] && s != dict[5]);
                dict[9] = display.Signals.Where(s => s.Length == length069).First(s => dict[4].All(c => s.Contains(c)));
                dict[0] = display.Signals.Where(s => s.Length == length069 && s != dict[9]).First(s => dict[7].All(c => s.Contains(c)));
                dict[6] = display.Signals.First(s => s.Length == length069 && s != dict[9] && s != dict[0]);
                var result = string.Concat(display.Output.Select(o => dict.First(p => o.Length == p.Value.Length && o.All(c => p.Value.Contains(c))).Key).Select(i => i.ToString()));
                return int.Parse(result);
            }).ToString());
        }
    }
}
