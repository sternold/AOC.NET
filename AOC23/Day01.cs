using AOC22.Shared;
using System.Collections.Frozen;

namespace AOC22
{
    internal class Day01 : BaseDay
    {
        private static readonly FrozenDictionary<string, int> Numbers = new Dictionary<string, int>()
        {
            { "1", 1 },
            { "one", 1 },
            { "2", 2 },
            { "two", 2 },
            { "3", 3 },
            { "three", 3 },
            { "4", 4 },
            { "four", 4 },
            { "5", 5 },
            { "five", 5 },
            { "6", 6 },
            { "six", 6 },
            { "7", 7 },
            { "seven", 7 },
            { "8", 8 },
            { "eight", 8 },
            { "9", 9 },
            { "nine", 9 },
            { "0", 0 },
            { "zero", 0 },
        }.ToFrozenDictionary();

        private readonly string _input;

        public Day01()
        {
            _input = FileReader.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
            => ValueTask.FromResult(_input.Split("\r\n").Sum(line =>
                {
                    char? f = null, l = null;
                    for (var i = 0; i < line.Length; i++)
                    {
                        f ??= char.IsDigit(line[i]) ? line[i] : null;
                        l ??= char.IsDigit(line[^(i + 1)]) ? line[^(i + 1)] : null;
                    }
                    return int.Parse($"{f}{l}");
                }).ToString());

        public override ValueTask<string> Solve_2()
            => ValueTask.FromResult(_input.Split("\r\n").Sum(line =>
                {
                    int? f = null, l = null;
                    for (var i = 0; i <= line.Length; i++)
                    {
                        foreach (var pair in Numbers)
                        {
                            f ??= string.Equals(line.Substring(i, int.Min(pair.Key.Length, line.Length - i)), pair.Key) ? pair.Value : null;
                            l ??= string.Equals(line.Substring(line.Length - i, int.Min(pair.Key.Length, i)), pair.Key) ? pair.Value : null;
                        }
                    }
                    return int.Parse($"{f}{l}");
                }).ToString());
    }
}
