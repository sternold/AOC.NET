namespace AOC21
{
    public class Day06 : BaseDay
    {
        public IEnumerable<int> Input { get; }

        public Day06()
        {
            Input = FileReader.ReadAllLines(InputFilePath).First().Split(',').Select(num => int.Parse(num));
        }

        public override ValueTask<string> Solve_1()
        {
            return new(GenerateLanternfish(Input, 80).Sum(kvp => kvp.Value).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            return new(GenerateLanternfish(Input, 256).Sum(kvp => kvp.Value).ToString());
        }

        public static IDictionary<int, long> GenerateLanternfish(IEnumerable<int> seed, int size)
        {
            var lanternfish = seed.ToLookup(num => num).ToDictionary(grp => grp.Key, grp => grp.LongCount());
            for (var i = 0; i < size; i++)
            {
                var births = 0L;
                for (var j = 0; j < 9; j++)
                {
                    if (!lanternfish.ContainsKey(j))
                    {
                        continue;
                    }
                    if (j == 0)
                    {
                        births = lanternfish[j];
                    }
                    else
                    {
                        lanternfish.Add(j - 1, lanternfish[j]);
                    }
                    lanternfish.Remove(j);
                }
                if (!lanternfish.TryAdd(6, births))
                {
                    lanternfish[6] += births;
                }
                lanternfish.Add(8, births);
            }
            return lanternfish;
        }
    }
}
