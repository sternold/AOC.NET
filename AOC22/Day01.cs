using AOC22.Shared;

namespace AOC22
{
    internal class Day01 : BaseDay
    {

        private readonly string _input;

        public Day01()
        {
            _input = FileReader.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1() => new(_input
            .Split((Environment.NewLine + Environment.NewLine), StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            .Max(arr => arr.Sum(s => int.Parse(s)))
            .ToString());

        public override ValueTask<string> Solve_2() => new(_input
            .Split((Environment.NewLine + Environment.NewLine), StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            .Select(arr => arr.Sum(s => int.Parse(s)))
            .OrderByDescending(v => v)
            .Take(3)
            .Sum()
            .ToString());
    }
}
