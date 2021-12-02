namespace AOC21
{
    public class Day02 : BaseDay
    {
        public IEnumerable<(string Direction, int Distance)> Input { get; }

        public Day02()
        {
            Input = FileReader.ReadAllLines(InputFilePath).Select(l => l.Split(' ')).Select(arr => (arr[0], int.Parse(arr[1])));
        }

        public override ValueTask<string> Solve_1()
        {
            return new(Input.Aggregate((H: 0, D: 0), (agg, comm) =>
            {
                switch (comm.Direction)
                {
                    case "forward":
                        agg.H += comm.Distance;
                        break;
                    case "up":
                        agg.D -= comm.Distance;
                        break;
                    case "down":
                        agg.D += comm.Distance;
                        break;
                }
                return agg;
            }, (agg) => agg.H * agg.D).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            return new(Input.Aggregate((H: 0, D: 0, A: 0), (agg, comm) =>
            {
                switch (comm.Direction)
                {
                    case "forward":
                        agg.H += comm.Distance;
                        agg.D += agg.A * comm.Distance;
                        break;
                    case "up":
                        agg.A -= comm.Distance;
                        break;
                    case "down":
                        agg.A += comm.Distance;
                        break;
                }
                return agg;
            }, (agg) => agg.H * agg.D).ToString());
        }
    }
}
