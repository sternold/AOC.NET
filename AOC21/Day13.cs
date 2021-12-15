namespace AOC21
{
    public class Day13 : BaseDay
    {
        public (char[,] Grid, IEnumerable<(int X, int Y)> Folds) Input { get; }

        public Day13()
        {
            var p = FileReader.ReadAllParagraphs(InputFilePath);
            var coords = p.First().Select(l => l.Split(',')).Select(arr => (X: int.Parse(arr[0]), Y: int.Parse(arr[1])));
            var grid = coords.Aggregate(new char[coords.Max(c => c.X)+1, coords.Max(c => c.Y)+1], (agg, next) =>
            {
                agg[next.X, next.Y] = '#';
                return agg;
            });
            var folds = p.Last().Select(l => l.Split(' ').Last().Split('=')).Select(arr => (X: arr[0] == "x" ? int.Parse(arr[1]) : 0, Y: arr[0] == "y" ? int.Parse(arr[1]) : 0));
            Input = (grid, folds);
        }

        public override ValueTask<string> Solve_1()
        {
            return new(Fold(Input.Grid, Input.Folds.First()).Cast<char>().Count(c => c == '#').ToString());
        }

        public override async ValueTask<string> Solve_2()
        {
            var result = Input.Folds.Aggregate(Input.Grid, (agg, next) => Fold(agg, next));
            for (int x = 0; x < result.GetLength(0); x++)
            {
                for (int y = 0; y < result.GetLength(1); y++)
                {
                    if (result[x, y] != default)
                    {
                        await GridFunctions.WriteAsync(x, y, result[x, y]);
                    }
                }
            }
            Console.WriteLine();
            return new("See Console");
        }

        public char[,] Fold(char[,] grid, (int X, int Y) fold)
        {
            var maxX = fold.X > 0 ? fold.X + 1 : grid.GetLength(0);
            var maxY = fold.Y > 0 ? fold.Y + 1 : grid.GetLength(1);
            var result = new char[maxX, maxY];
            for(int x = 0; x < maxX; x++)
            {
                for(int y = 0; y < maxY; y++)
                {
                    if (grid[x, y] != default)
                    {
                        result[x, y] = grid[x, y];
                    }
                }
            }
            for (int x = fold.X; x < grid.GetLength(0); x++)
            {
                for (int y = fold.Y; y < grid.GetLength(1); y++)
                {
                    if(grid[x, y] != default)
                    {
                        if (fold.X > 0)
                        {
                            result[fold.X - (x - fold.X), y] = grid[x, y];
                        }
                        if (fold.Y > 0)
                        {
                            result[x, fold.Y - (y - fold.Y)] = grid[x, y];
                        }
                    }
                }
            }
            return result;
        }
    }
}
