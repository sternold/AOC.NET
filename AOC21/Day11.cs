namespace AOC21
{
    public class Day11 : BaseDay
    {
        public int[,] Input { get; }

        public Day11()
        {
            Input = FileReader.ReadAsGrid(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            return new(Enumerable.Range(1, 100).Aggregate((Grid: (int[,])Input.Clone(), Flashes: 0), (agg, next)
                => Enumerable.Range(0, agg.Grid.GetLength(0)).Aggregate((Aggregrate: agg, Flashed: new bool[agg.Grid.GetLength(0), agg.Grid.GetLength(1)]), (xagg, x)
                => Enumerable.Range(0, agg.Grid.GetLength(1)).Aggregate((xagg.Aggregrate, xagg.Flashed), (yagg, y)
                =>
                {
                    if (!yagg.Flashed[x, y]) yagg.Aggregrate.Grid[x, y]++;
                    else return yagg;
                    if (yagg.Aggregrate.Grid[x, y] > 9) yagg.Aggregrate.Flashes += Flash(yagg.Aggregrate.Grid, x, y, yagg.Flashed);
                    else return yagg;
                    return yagg;
                })).Aggregrate, (agg) => agg.Flashes).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            return new(Enumerable.Range(1, 1000).Aggregate((Grid: (int[,])Input.Clone(), SyncStep: new int?()), (agg, next)
                =>
                {
                    if (agg.SyncStep != null)
                    {
                        return agg;
                    }
                    var flashed = new bool[agg.Grid.GetLength(0), agg.Grid.GetLength(1)];
                    var step = Enumerable.Range(0, agg.Grid.GetLength(0)).Aggregate((Aggregrate: agg, Flashed: flashed), (xagg, x)
                          => Enumerable.Range(0, agg.Grid.GetLength(1)).Aggregate((xagg.Aggregrate, xagg.Flashed), (yagg, y)
                          =>
                          {
                              if (!yagg.Flashed[x, y]) yagg.Aggregrate.Grid[x, y]++;
                              else return yagg;
                              if (yagg.Aggregrate.Grid[x, y] > 9) Flash(yagg.Aggregrate.Grid, x, y, yagg.Flashed);
                              else return yagg;
                              return yagg;
                          }));
                    if (step.Aggregrate.SyncStep == null && step.Flashed.Cast<bool>().All(f => f))
                    {
                        step.Aggregrate.SyncStep = next;
                    }
                    return step.Aggregrate;
                }, (agg) => agg.SyncStep ?? 0).ToString());
        }

        public int Flash(int[,] grid, int x, int y, bool[,] flashed)
        {
            flashed[x, y] = true;
            grid[x, y] = 0;
            return 1 + GridFunctions.Neighbours(grid, x, y, diagonals: true).Sum(nb =>
            {
                if (!flashed[nb.X, nb.Y]) grid[nb.X, nb.Y]++;
                else return 0;
                if (grid[nb.X, nb.Y] > 9) return Flash(grid, nb.X, nb.Y, flashed);
                else return 0;
            });
        }
    }
}
