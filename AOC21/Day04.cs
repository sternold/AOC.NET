namespace AOC21
{
    public class Day04 : BaseDay
    {
        public IEnumerable<string> Input { get; }
        public IEnumerable<int> Draws { get; }
        public IEnumerable<int[][]> Boards { get; }

        public Day04()
        {
            Input = FileReader.ReadAllLines(InputFilePath);
            var groups = new List<List<string>>();
            groups.Add(new List<string>());
            for(int i = 0; i < Input.Count(); i++)
            {
                var line = Input.ElementAt(i);
                if (string.IsNullOrEmpty(line))
                {
                    groups.Add(new List<string>());
                }
                else
                {
                    groups.Last().Add(line);
                }
            }
            Draws = groups.First().First().Split(",").Select(num => int.Parse(num)).ToList();
            Boards = groups.Skip(1).Select(group => group.Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(num => int.Parse(num)).ToArray()).ToArray()).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            var boards = new List<int[][]>(Boards);
            foreach (var draw in Draws) 
            {
                for(int i = 0; i < boards.Count; i++)
                {
                    boards[i] = MarkNumber(boards[i], draw);
                    if (CheckWinCondition(boards[i]))
                    {
                        return new((boards[i].Sum(row => row.Sum(num => num > 0 ? num : 0)) * draw).ToString());
                    }
                }
            }
            return new("No Solution");
        }

        public override ValueTask<string> Solve_2()
        {
            var winners = new HashSet<int>();
            var boards = new List<int[][]>(Boards);
            foreach (var draw in Draws)
            {
                for (int i = 0; i < boards.Count; i++)
                {
                    boards[i] = MarkNumber(boards[i], draw);
                    if (CheckWinCondition(boards[i]))
                    {
                        winners.Add(i);
                    }
                }
                if(winners.Count == boards.Count)
                {
                    return new((boards[winners.Last()].Sum(row => row.Sum(num => num > 0 ? num : 0)) * draw).ToString());
                }
            }
            return new("No Solution");
        }

        public int[][] MarkNumber(int[][] board, int number)
        {
            for (int i = 0; i < board.Length; i++)
            {
                var row = board[i];
                for (int j = 0; j < row.Length; j++)
                {
                    if(row[j] == number)
                    {
                        board[i][j] *= -1;
                    }
                }
            }
            return board;
        }

        public bool CheckWinCondition(int[][] board)
        {
            return board.Any(row => row.All(num => num < 0)) || board.First().Select((num, i) => board.All(row => row[i] < 0)).Any(result => result);
        }
    }
}
