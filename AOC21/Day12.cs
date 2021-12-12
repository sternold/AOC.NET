namespace AOC21
{
    public class Day12 : BaseDay
    {
        public Graph Input { get; }

        public Day12()
        {
            Input = FileReader.ReadAllLines(InputFilePath).Aggregate(new Graph(), (agg, next) => agg.InsertPath(next));
        }

        public override ValueTask<string> Solve_1()
        {
            var results = new List<IEnumerable<Graph.Node>>();
            var queue = new Queue<IEnumerable<Graph.Node>>();
            queue.Enqueue(new List<Graph.Node> { Input.Start });
            while(queue.Count > 0)
            {
                var path = queue.Dequeue();
                var node = path.Last();
                if (node.Equals(Input.End))
                {
                    results.Add(path);
                }
                else
                {
                    foreach (var connectedNode in node.ConnectsTo.Where((n) => n.Large || !path.Contains(n)))
                    {
                        var newPath = new List<Graph.Node>(path)
                        {
                            connectedNode
                        };
                        queue.Enqueue(newPath);
                    }
                }
            }

            return new(results.Count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var results = new List<IEnumerable<Graph.Node>>();
            var queue = new Queue<IEnumerable<Graph.Node>>();
            queue.Enqueue(new List<Graph.Node> { Input.Start });
            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                var node = path.Last();
                if (node.Equals(Input.End))
                {
                    results.Add(path);
                }
                else
                {
                    var hasVisitedSmallTwice = new HashSet<Graph.Node>(path.Where(n => n.Id != n.Id.ToUpper())).Count == path.Count(n => n.Id != n.Id.ToUpper());
                    foreach (var connectedNode in node.ConnectsTo.Where((n) => n.Large || (!n.Equals(Input.Start) && (hasVisitedSmallTwice || !path.Contains(n)))))
                    {
                        var newPath = new List<Graph.Node>(path)
                        {
                            connectedNode
                        };
                        queue.Enqueue(newPath);
                    }
                }
            }

            return new(results.Count.ToString());
        }

        public class Graph
        {
            public record Node
            {
                public string Id { get; }
                public bool Large { get; }
                public ICollection<Node> ConnectsTo { get; }

                public Node(string id)
                {
                    Id = id;
                    Large = id == id.ToUpper();
                    ConnectsTo = new List<Node>();
                }
            }

            public Node? Start { get; private set; }
            public Node? End { get; private set; }
            public IDictionary<string, Node> Nodes { get; }

            public Graph()
            {
                Nodes = new Dictionary<string, Node>();
            }

            public Graph InsertPath(string path)
            {
                var ids = path.Split('-', StringSplitOptions.RemoveEmptyEntries);

                if (!Nodes.TryGetValue(ids[0], out Node? lnode))
                {
                    lnode = new Node(ids[0]);
                    Nodes.Add(ids[0], lnode);
                    if (ids[0] == "start")
                    {
                        Start = lnode;
                    }
                    else if (ids[0] == "end")
                    {
                        End = lnode;
                    }
                }

                if (!Nodes.TryGetValue(ids[1], out Node? rnode))
                {
                    rnode = new Node(ids[1]);
                    Nodes.Add(ids[1], rnode);
                    if (ids[1] == "start")
                    {
                        Start = rnode;
                    }
                    else if (ids[1] == "end")
                    {
                        End = rnode;
                    }
                }

                lnode.ConnectsTo.Add(rnode);
                rnode.ConnectsTo.Add(lnode);
                return this;
            }
        }
    }
}
