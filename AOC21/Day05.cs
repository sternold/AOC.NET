namespace AOC21
{
    public class Day05 : BaseDay
    {
        public IEnumerable<((int X, int Y) LHS,(int X,int Y) RHS)> Input { get; }

        public Day05()
        {
            Input = FileReader.ReadAllLines(InputFilePath)
                .Select(line => line.Split(" -> "))
                .Select(coordsArr => coordsArr.Select(coords => coords.Split(",").Select(coord => int.Parse(coord)).ToArray()).ToArray())
                .Select(coordsArr => ((coordsArr[0][0], coordsArr[0][1]),(coordsArr[1][0], coordsArr[1][1])));
        }

        public override ValueTask<string> Solve_1()
        {
            var grid = new int[1000,1000];
            foreach(var coords in Input)
            {
                if(coords.LHS.X == coords.RHS.X || coords.LHS.Y == coords.RHS.Y)
                {
                    var xDiff = coords.LHS.X - coords.RHS.X;
                    var yDiff = coords.LHS.Y - coords.RHS.Y;
                    for(int x = 0; x < Math.Abs(xDiff)+1; x++)
                    {
                        for (int y = 0; y < Math.Abs(yDiff)+1; y++)
                        {
                            var xCoord = coords.LHS.X + ((xDiff < 0) ? x : x*-1);
                            var yCoord = coords.LHS.Y + ((yDiff < 0) ? y : y*-1);
                            grid[xCoord, yCoord]++;
                        }
                    }
                }
            }
            return new(grid.Cast<int>().Count(c => c >= 2).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var grid = new int[1000, 1000];
            foreach (var coords in Input)
            {
                if (coords.LHS.X == coords.RHS.X || coords.LHS.Y == coords.RHS.Y)
                {
                    var xDiff = coords.LHS.X - coords.RHS.X;
                    var yDiff = coords.LHS.Y - coords.RHS.Y;
                    for (int x = 0; x < Math.Abs(xDiff) + 1; x++)
                    {
                        for (int y = 0; y < Math.Abs(yDiff) + 1; y++)
                        {
                            var xCoord = coords.LHS.X + ((xDiff < 0) ? x : x * -1);
                            var yCoord = coords.LHS.Y + ((yDiff < 0) ? y : y * -1);
                            grid[xCoord, yCoord]++;
                        }
                    }
                }
                else
                {
                    var diff = coords.LHS.X - coords.RHS.X;
                    var xDir = coords.LHS.X - coords.RHS.X;
                    var yDir = coords.LHS.Y - coords.RHS.Y;
                    for (int xy = 0; xy < Math.Abs(diff) + 1; xy++)
                    {
                        var xCoord = coords.LHS.X + ((xDir < 0) ? xy : xy * -1);
                        var yCoord = coords.LHS.Y + ((yDir < 0) ? xy : xy * -1);
                        grid[xCoord, yCoord]++;
                    }
                }
            }
            return new(grid.Cast<int>().Count(c => c >= 2).ToString());
        }
    }
}
