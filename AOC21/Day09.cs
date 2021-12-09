namespace AOC21
{
    public class Day09 : BaseDay
    {
        public int[,] Input { get; }

        public Day09()
        {
            var lines = FileReader.ReadAllLines(InputFilePath);
            Input = lines.Select((line, i) => (Value: line, Index: i))
                         .Aggregate(new int[lines.Count(), lines.First().Length], (agg, next)
                            => next.Value.Select((c, i) => (Value: c.ToString(), Index: i))
                                         .Aggregate(agg, (agg2, next2) =>
                                            {
                                                agg2[next.Index, next2.Index] = int.Parse(next2.Value);
                                                return agg2;
                                            }));
        }

        public override ValueTask<string> Solve_1()
        {
            var count = 0;
            for(int x = 0; x < Input.GetLength(0); x++)
            {
                for(int y = 0; y < Input.GetLength(1); y++)
                {
                    if(Neighbours(Input, x, y).All(n => n.Value > Input[x, y]))
                    {
                        count += 1+Input[x, y];
                    }
                }
            }
            return new(count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var visited = new HashSet<(int X, int Y)>();
            var basins = new List<IEnumerable<(int X, int Y)>>();
            for (int x = 0; x < Input.GetLength(0); x++)
            {
                for (int y = 0; y < Input.GetLength(1); y++)
                {
                    if (!visited.Contains((x, y)))
                    {
                        if (Neighbours(Input, x, y).All(n => n.Value > Input[x, y]))
                        {
                            var result = Visit(Input, (x, y), visited);
                            if (result.Any())
                            {
                                basins.Add(result);
                            }
                        }
                    }
                }
            }
            return new(basins.Select(b => b.Count()).OrderByDescending(c => c).Take(3).Aggregate(1, (agg, next) => agg*next).ToString());
        }

        public IEnumerable<(int X, int Y)> Visit(int[,] grid, (int X, int Y) node, HashSet<(int X, int Y)> visited)
        {
            var result = new List<(int X, int Y)>();
            if (visited.Contains(node))
            {
                return result;
            }
            visited.Add(node);
            result.Add(node);
            foreach (var childResult in Neighbours(grid, node.X, node.Y).Where(n => n.Value < 9).Select(n => Visit(grid, (n.X, n.Y), visited)))
            {
                result.AddRange(childResult);
            }
            return result;
        }

        public IEnumerable<(int X, int Y, int Value)> Neighbours(int[,] grid, int x, int y)
        {
            if (x - 1 >= 0) yield return (x - 1, y, grid[x - 1, y]);
            if (x + 1 < grid.GetLength(0)) yield return (x + 1, y, grid[x + 1, y]);
            if (y - 1 >= 0) yield return (x, y - 1, grid[x, y - 1]);
            if (y + 1 < grid.GetLength(1)) yield return (x, y + 1, grid[x, y + 1]);
        }
    }
}
