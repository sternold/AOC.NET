using System.Collections;

namespace AOC21
{
    public class Day03 : BaseDay
    {
        public IEnumerable<bool[]> Input { get; }

        public Day03()
        {
            Input = FileReader.ReadAllLines(InputFilePath).Select(l => l.ToCharArray()).Select(arr => arr.Select(c => Convert.ToBoolean(int.Parse(c.ToString()))).ToArray()).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            return new(Input.Aggregate(new int[Input.First().Length], (agg, bits) => agg.Select((n, i) => bits[i] ? n+1 : n-1).ToArray(), (agg) =>
            {
                var bits = new BitArray(agg.Reverse().Select(c => c > 0).ToArray());
                var numbers = new uint[2];
                bits.CopyTo(numbers, 0);
                bits.Not().CopyTo(numbers, 1);
                return numbers[0] * numbers[1];
            }).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var oxygenFilter = RecursiveFilter(Input, 0, (i) => i == 0 || i > 0);
            var co2Filter = RecursiveFilter(Input, 0, (i) => i != 0 && i < 0);
            var numbers = new uint[2];
            new BitArray(oxygenFilter.First().Reverse().ToArray()).CopyTo(numbers, 0);
            new BitArray(co2Filter.First().Reverse().ToArray()).CopyTo(numbers, 1);
            return new((numbers[0] * numbers[1]).ToString());
        }

        private IEnumerable<bool[]> RecursiveFilter(IEnumerable<bool[]> bytes, int index, Func<int, bool> comparator)
            => bytes.Count() > 1
            ? RecursiveFilter(bytes.Aggregate(0, (agg, bits) => bits[index] ? agg+1 : agg-1, (agg) => bytes.Where(bits => bits[index] == comparator.Invoke(agg))), index+1, comparator)
            : bytes;
    }
}
