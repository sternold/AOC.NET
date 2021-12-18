namespace AOC21
{
    public class Day18 : BaseDay
    {
        public abstract record class SnailfishNumberElement(int Depth)
        {
            public int Depth { get; set; } = Depth;
            public SnailfishNumberElement? Parent { get; set; }
            public virtual bool Modified { get; set; }
            public abstract long Magnitude { get; }
            public abstract bool TryReduce(out SnailfishNumberElement element, out long? toLeft, out long? toRight);
            public abstract bool TryAdd(long value, bool left);
            public abstract void IncreaseDepth();
        }

        public record class SnailfishNumber(int Depth, SnailfishNumberElement LHS, SnailfishNumberElement RHS) : SnailfishNumberElement(Depth)
        {
            public override long Magnitude => (LHS.Magnitude * 3) + (RHS.Magnitude * 2);
            public SnailfishNumberElement LHS { get; private set; } = LHS;
            public SnailfishNumberElement RHS { get; private set; } = RHS;
            public override bool Modified => LHS.Modified || RHS.Modified;
            public bool TryReduce(out SnailfishNumber element)
            {
                bool tried = false;
                bool doTry = true;
                while (doTry)
                {
                    doTry = false;
                    if (LHS.TryReduce(out var lhsResult, out var lhsToLeft, out var lhsToRight))
                    {
                        tried = true;
                        doTry = true;
                        LHS = lhsResult;
                        if (lhsToRight.HasValue) RHS.TryAdd(lhsToRight.Value, true);
                        if (lhsToRight.HasValue) Parent?.TryAdd(lhsToRight.Value, true);
                        if (lhsToLeft.HasValue) Parent?.TryAdd(lhsToLeft.Value, false);
                    }
                    if (RHS.TryReduce(out var rhsResult, out var rhsToLeft, out var rhsToRight))
                    {
                        tried = true;
                        doTry = true;
                        RHS = rhsResult;
                        if (rhsToLeft.HasValue) LHS.TryAdd(rhsToLeft.Value, false);
                        if (rhsToLeft.HasValue) Parent?.TryAdd(rhsToLeft.Value, false);
                        if (rhsToRight.HasValue) Parent?.TryAdd(rhsToRight.Value, true);
                    }
                }
                element = this;
                return tried;
            }

            public override bool TryReduce(out SnailfishNumberElement element, out long? toLeft, out long? toRight)
            {
                toLeft = null;
                toRight = null;

                TryReduce(out var reduced);
                if (Depth >= 4)
                {
                    toLeft = ((SnailfishValue)reduced.LHS).Value;
                    toRight = ((SnailfishValue)reduced.RHS).Value;
                    element = new SnailfishValue(Depth, 0L);
                    return true;
                }
                else
                {
                    element = reduced;
                    return false;
                }
            }

            public override bool TryAdd(long value, bool left)
            {
                if (!(left ? RHS.TryAdd(value, left) : LHS.TryAdd(value, left))
                    && !(left ? LHS.TryAdd(value, left) : RHS.TryAdd(value, left)))
                    return Parent?.TryAdd(value, left) ?? false;
                return true;
            }

            public override void IncreaseDepth()
            {
                Depth++;
                LHS.IncreaseDepth();
                RHS.IncreaseDepth();
            }

            public override string ToString() => $"[{LHS},{RHS}]";

            public static SnailfishNumber operator +(SnailfishNumber lhs, SnailfishNumber rhs)
            {
                var depth = lhs.Depth;
                lhs.IncreaseDepth();
                rhs.IncreaseDepth();
                return new(depth, lhs, rhs);
            }
        }

        public record class SnailfishValue(int Depth, long Value) : SnailfishNumberElement(Depth)
        {
            public long Value { get; set; } = Value;

            public override bool TryReduce(out SnailfishNumberElement element, out long? toLeft, out long? toRight)
            {
                toLeft = null;
                toRight = null;

                if (Value >= 10)
                {
                    var lhsValue = (long)Math.Floor(((float)Value) / 2);
                    var rhsValue = (long)Math.Ceiling(((float)Value) / 2);
                    element = new SnailfishNumber(Depth, new SnailfishValue(Depth + 1, lhsValue), new SnailfishValue(Depth + 1, rhsValue));
                    return true;
                }
                else
                {
                    element = this;
                    return false;
                }
            }

            public override long Magnitude => Value;

            public override bool TryAdd(long value, bool left)
            {
                Modified = true;
                Value += value;
                return true;
            }

            public override void IncreaseDepth()
            {
                Depth++;
            }

            public override string ToString() => $"{Value}";
        }

        public IEnumerable<SnailfishNumber> Input { get; }

        public Day18()
        {
            Input = FileReader.ReadAllLines(InputFilePath).Select(line => ParseSnailfishNumber(line, 1)).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            var result = Input.Aggregate((agg, next) =>
            {
                agg = (agg + next);
                while (agg.TryReduce(out var el) && el.Modified)
                {
                    agg = el;
                }
                return agg;
            });
            Console.WriteLine(result);
            return new(result.Magnitude.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            return new("Solution");
        }

        public SnailfishNumber ParseSnailfishNumber(string toParse, int depth)
        {
            var split = Split(toParse);
            SnailfishNumberElement LHS = split[0].StartsWith('[')
                ? ParseSnailfishNumber(split[0], depth + 1)
                : new SnailfishValue(depth, long.Parse(split[0]));
            SnailfishNumberElement RHS = split[1].StartsWith('[')
                ? ParseSnailfishNumber(split[1], depth + 1)
                : new SnailfishValue(depth, long.Parse(split[1]));
            SnailfishNumber parent = new(depth, LHS, RHS);
            LHS.Parent = parent;
            RHS.Parent = parent;
            return parent;
        }

        public string[] Split(string toSplit)
        {
            toSplit = toSplit.Substring(1, toSplit.Length - 2);
            var result = new string[2];
            int pairCount = 0;
            int? commaIndex = null;
            for (int i = 0; i < toSplit.Length; i++)
            {
                if (toSplit[i] == ',')
                {
                    if (pairCount == 0)
                    {
                        commaIndex = i;
                        result[0] = toSplit.Substring(0, i);
                    }
                }
                else if (toSplit[i] == '[')
                {
                    pairCount++;
                }
                else if (toSplit[i] == ']')
                {
                    pairCount--;
                }
            }
            result[1] = toSplit.Substring(commaIndex.Value + 1, toSplit.Length - commaIndex.Value - 1);
            return result;
        }
    }
}
