namespace AOC21
{
    public class Day15 : BaseDay
    {
        public int[,] Input { get; }

        public Day15()
        {
            Input = FileReader.ReadAsGrid(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            return new(CalculateCost(Input, new(0, 0), new(Input.GetLength(0)-1, Input.GetLength(1)-1)).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var expandedGrid = ExpandGrid(Input, 5);
            return new(CalculateCost(expandedGrid, new(0, 0), new(expandedGrid.GetLength(0) - 1, expandedGrid.GetLength(1) - 1)).ToString());
        }

        public int[,] ExpandGrid(int[,] grid, int factor)
        {
            var newGrid = new int[grid.GetLength(0) * factor, grid.GetLength(1) * factor];
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    for (int dx = 0; dx < factor; dx++)
                    {
                        for (int dy = 0; dy < factor; dy++)
                        {
                            var newValue = (grid[x, y] + dx + dy);
                            newGrid[x + (grid.GetLength(0) * dx), y + (grid.GetLength(1) * dy)] = newValue > 9 ? newValue - 9 : newValue;
                        }
                    }
                }
            }
            return newGrid;
        }

        public long CalculateCost(int[,] grid, Coords start, Coords end)
        {
            bool[,] visited = new bool[grid.GetLength(0), grid.GetLength(1)];
            var queue = new PriorityQueue<(IEnumerable<Coords> Coords, long Cost), long>(new List<((IEnumerable<Coords> Coords, long Cost), long)>{ ((new Coords[] { start }, 0L), 0L) });
            while(queue.TryDequeue(out var coords, out var priority))
            {
                var currentCoord = coords.Coords.Last();
                if (visited[currentCoord.X, currentCoord.Y]) continue;
                else visited[currentCoord.X, currentCoord.Y] = true;
                if (currentCoord.X == end.X && currentCoord.Y == end.Y) {
                    return coords.Cost;
                        }
                foreach(var nb in GridFunctions.Neighbours(grid, currentCoord.X, currentCoord.Y, false))
                {
                    if (!coords.Coords.Contains(nb))
                        queue.Enqueue((new List<Coords>(coords.Coords).Append(nb), coords.Cost + nb.Value), (end.X - nb.X) + (end.Y - nb.Y) + coords.Cost + nb.Value);
                }
            }
            return 0L;
        }
    }
}
