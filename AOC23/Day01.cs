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

        public override ValueTask<string> Solve_1() => new();

        public override ValueTask<string> Solve_2() => new();
    }
}
