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
            var queue = new PriorityQueue<(Coords Coord, long Cost), long>(new List<((Coords, long), long)>{ ((start, 0L), 0L) });
            while(queue.TryDequeue(out var current, out var priority))
            {
                if (visited[current.Coord.X, current.Coord.Y]) continue;
                else visited[current.Coord.X, current.Coord.Y] = true;
                if (current.Coord.X == end.X && current.Coord.Y == end.Y) return current.Cost;
                else foreach(var nb in GridFunctions.Neighbours(grid, current.Coord.X, current.Coord.Y, false))
                {
                    queue.Enqueue((nb, current.Cost + nb.Value), (end.X - nb.X) + (end.Y - nb.Y) + current.Cost + nb.Value);
                }
            }
            return 0L;
        }
    }
}
