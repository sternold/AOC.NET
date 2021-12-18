using System.Numerics;
using System.Text.RegularExpressions;

namespace AOC21
{
    public class Day17 : BaseDay
    {
        public record struct Box(Vector2 Min, Vector2 Max)
        {
            public float Left = Min.X;
            public float Right = Max.X;
            public float Top = Min.Y;
            public float Bottom = Max.Y;
            public bool Contains(Vector2 cv) => cv.X >= Left
                                             && cv.X <= Right
                                             && cv.Y >= Bottom
                                             && cv.Y <= Top;
        }

        public Box Input { get; }

        public Day17()
        {
            var regex = new Regex(@"target area: x=(?<X>(?<XLHS>-?\d+)\.\.(?<XRHS>-?\d+)), y=(?<Y>(?<YLHS>-?\d+)\.\.(?<YRHS>-?\d+))");
            var match = regex.Match(FileReader.ReadAllLines(InputFilePath).Single());
            var xlhs = int.Parse(match.Groups.GetValueOrDefault("XLHS")?.Value ?? string.Empty);
            var xrhs = int.Parse(match.Groups.GetValueOrDefault("XRHS")?.Value ?? string.Empty);
            var ylhs = int.Parse(match.Groups.GetValueOrDefault("YLHS")?.Value ?? string.Empty);
            var yrhs = int.Parse(match.Groups.GetValueOrDefault("YRHS")?.Value ?? string.Empty);
            Input = new(new(xlhs, yrhs), new(xrhs, ylhs));
        }

        public override ValueTask<string> Solve_1()
        {
            for(int y = byte.MaxValue; y >= 0; y--)
            {
                for(int x = 0; x < byte.MaxValue; x++)
                {
                    var peak = Fire(Vector2.Zero, new(x, y), Input);
                    if (peak != null) return new(peak.ToString() ?? string.Empty);
                }
            }
            return new("Not Found");
        }

        public override ValueTask<string> Solve_2()
        {
            int count = 0;
            for (int y = -byte.MaxValue; y < byte.MaxValue; y++)
            {
                for (int x = 0; x < byte.MaxValue; x++)
                {
                    var peak = Fire(Vector2.Zero, new(x, y), Input);
                    if (peak != null) count++;
                }
            }
            return new(count.ToString());
        }

        public static float? Fire(Vector2 origin, Vector2 velocity, Box target)
        {
            float peak = 0;
            var coordinates = origin;
            while(coordinates.X <= target.Right && coordinates.Y >= target.Bottom)
            {
                coordinates += velocity;
                if (coordinates.Y > peak) peak = coordinates.Y;
                velocity = new(Math.Max(velocity.X - 1, 0), velocity.Y - 1);
                if (target.Contains(coordinates)) return peak;
            }
            return null;
        }
    }
}
