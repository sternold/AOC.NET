namespace AOC21.Shared
{
    public record Coords(int X, int Y);
    public record ValueCoords(int X, int Y, int Value) : Coords(X, Y);

    public static class GridFunctions
    {
        public static IEnumerable<ValueCoords> Neighbours(int[,] grid, int x, int y, bool diagonals)
        {
            if (x - 1 >= 0) yield return new(x - 1, y, grid[x - 1, y]);
            if (x + 1 < grid.GetLength(0)) yield return new(x + 1, y, grid[x + 1, y]);
            if (y - 1 >= 0) yield return new(x, y - 1, grid[x, y - 1]);
            if (y + 1 < grid.GetLength(1)) yield return new(x, y + 1, grid[x, y + 1]);
            if (diagonals)
            {
                if (x - 1 >= 0 && y - 1 >= 0) yield return new(x - 1, y - 1, grid[x - 1, y - 1]);
                if (x + 1 < grid.GetLength(0) && y - 1 >= 0) yield return new(x + 1, y - 1, grid[x + 1, y - 1]);
                if (x - 1 >= 0 && y + 1 < grid.GetLength(1)) yield return new(x - 1, y + 1, grid[x - 1, y + 1]);
                if (x + 1 < grid.GetLength(0) && y + 1 < grid.GetLength(1)) yield return new(x + 1, y + 1, grid[x + 1, y + 1]);
            }
        }

        public static async Task WriteAsync(int x, int y, char c)
        {
            Console.SetCursorPosition(x, y);
            await Console.Out.WriteAsync(c);
        }
    }
}
