namespace AOC21
{
    public class Day09 : BaseDay
    {
        public int[,] Input { get; }

        public Day09()
        {
            Input = FileReader.ReadAsGrid(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            var count = 0;
            for(int x = 0; x < Input.GetLength(0); x++)
            {
                for(int y = 0; y < Input.GetLength(1); y++)
                {
                    if(GridFunctions.Neighbours(Input, x, y, false).All(n => n.Value > Input[x, y]))
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
                        if (GridFunctions.Neighbours(Input, x, y, diagonals: false).All(n => n.Value > Input[x, y]))
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
            foreach (var childResult in GridFunctions.Neighbours(grid, node.X, node.Y, diagonals: false).Where(n => n.Value < 9).Select(n => Visit(grid, (n.X, n.Y), visited)))
            {
                result.AddRange(childResult);
            }
            return result;
        }
    }
}
