namespace AOC21
{
    public class Day07 : BaseDay
    {
        public IEnumerable<int> Input { get; }

        public Day07()
        {
            Input = FileReader.ReadAllLines(InputFilePath).First().Split(',').Select(num => int.Parse(num));
        }

        public override ValueTask<string> Solve_1()
        {
            return new(Enumerable.Range(Input.Min(), Input.Max() - Input.Min()).Min((i) => Input.Select(num => Math.Abs(num - i)).Sum()).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            return new(Enumerable.Range(Input.Min(), Input.Max() - Input.Min()).Min((i) => Input.Select(num => Math.Abs(num - i)).Select(dis => dis * (dis + 1) / 2).Sum()).ToString());
        }
    }
}
