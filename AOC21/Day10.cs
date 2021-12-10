namespace AOC21
{
    public class Day10 : BaseDay
    {
        public static readonly Dictionary<char, char> Pairs = new()
        {
            { '<', '>' },
            { '[', ']' },
            { '{', '}' },
            { '(', ')' }
        };

        public IEnumerable<string> Input { get; }

        public Day10()
        {
            Input = FileReader.ReadAllLines(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            return new(FindCorruptSymbols(Input)
                       .Where(line => line.Corrupt.HasValue)
                       .Sum(line => line.Corrupt switch
                       {
                           ')' => 3,
                           ']' => 57,
                           '}' => 1197,
                           '>' => 25137,
                           _ => 0
                       })
                       .ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            return new(FindCorruptSymbols(Input)
                       .Where(line => !line.Corrupt.HasValue)
                       .Select(line => line.Stack.Select(c => Pairs[c])
                                                 .Aggregate(0L,
                                                 (agg, next) => (agg * 5) + (next switch
                                                 {
                                                     ')' => 1L,
                                                     ']' => 2L,
                                                     '}' => 3L,
                                                     '>' => 4L,
                                                     _ => 0L
                                                 })))
                       .Median()
                       .ToString());
        }

        public static IEnumerable<(char? Corrupt, Stack<char> Stack)> FindCorruptSymbols(IEnumerable<string> lines)
            => lines.Select(line => FindCorruptSymbol(line));

        public static (char? Corrupt, Stack<char> Stack) FindCorruptSymbol(string line)
            => line.Aggregate((Result: new char?(), Stack: new Stack<char>()),
                (agg, next) =>
                {
                    if (agg.Result != null) return agg;
                    else if (Pairs.ContainsKey(next)) agg.Stack.Push(next);
                    else if (next != Pairs[agg.Stack.Pop()]) agg.Result = next;
                    return agg;
                });
    }
}
