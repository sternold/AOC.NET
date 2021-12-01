namespace AOC21
{
    public class Day01 : BaseDay
    {
        public IEnumerable<int> Input { get; }

        public Day01()
        {
            Input = FileReader.ReadAllLinesAsInteger(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            return new(Input.Skip(1).Zip(Input, (curr, prev) => curr > prev).Count(result => result).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var windows = Input.Zip(Input.Skip(1), Input.Skip(2)).Select((zipped) => zipped.First + zipped.Second + zipped.Third);
            return new(windows.Skip(1).Zip(windows, (curr, prev) => curr > prev).Count(result => result).ToString());
        }
    }
}
