namespace AOC21
{
    public class Day14 : BaseDay
    {
        public (Dictionary<char, long> Elements, Dictionary<string, long> Pairs, IEnumerable<(string Pair, char Insert)> Rules) Input { get; }

        public Day14()
        {
            var input = FileReader.ReadAllParagraphs(InputFilePath);
            var rules = input.Last()
                             .Select(line => line.Split("->"))
                             .Select(arr => (Pair: arr[0].Trim(), Insert: arr[1].Trim()[0]));
            var template = input.First().Single();
            var elements = template.GroupBy(c => c).ToDictionary(grp => grp.Key, grp => grp.LongCount());
            var templatePairs = template.Zip(template.Skip(1)).Select(pair => $"{pair.First}{pair.Second}");
            var pairs = new Dictionary<string, long>(rules.Select(rule => new KeyValuePair<string, long>(rule.Pair, templatePairs.LongCount(p => string.Equals(p, rule.Pair)))));
            foreach(var pair in pairs)
            {
                if(!elements.ContainsKey(pair.Key[0])){
                    elements.Add(pair.Key[0], 0);
                }
            }
            Input = (elements, pairs, rules);
        }

        public override ValueTask<string> Solve_1()
        {
            var elementCount = Insert(Input.Pairs, new(Input.Elements), 10);
            return new((elementCount.Max(p => p.Value) - elementCount.Min(p => p.Value)).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var elementCount = Insert(Input.Pairs, new(Input.Elements), 40);
            return new((elementCount.Max(p => p.Value) - elementCount.Min(p => p.Value)).ToString());
        }

        public Dictionary<char, long> Insert(Dictionary<string, long> pairs, Dictionary<char, long> elementCount, int steps)
        {
            for(int i = 0; i < steps; i++)
            {
                var newPairs = new Dictionary<string, long>(pairs);
                foreach(var rule in Input.Rules)
                {
                    var newPairLeft = $"{rule.Pair[0]}{rule.Insert}";
                    var newPairRight = $"{rule.Insert}{rule.Pair[1]}";
                    var addedNewPair = pairs.GetValueOrDefault(rule.Pair, 0L);
                    newPairs[rule.Pair] -= addedNewPair;
                    newPairs[newPairLeft] += addedNewPair;
                    newPairs[newPairRight] += addedNewPair;
                    elementCount[rule.Insert] += addedNewPair;
                }
                pairs = newPairs;
            }
            return elementCount;
        }
    }
}
