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
            var result = int.MaxValue;
            for(int i = Input.Min(); i <= Input.Max(); i++)
            {
                var diff = Input.Select(num => Math.Abs(num - i)).Sum();
                if(diff < result)
                {
                    result = diff;
                }
            }
            return new(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var result = int.MaxValue;
            for (int i = Input.Min(); i <= Input.Max(); i++)
            {
                var diff = Input.Select(num => Math.Abs(num - i)).Select(dis => dis*(dis+1)/2).Sum();
                if (diff < result)
                {
                    result = diff;
                }
            }
            return new(result.ToString());
        }
    }
}
