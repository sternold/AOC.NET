global using AoCHelper;

if (args.Length == 0)
{
    await Solver.SolveLast();
}
else if (args.Length == 1 && args[0].Contains("all", StringComparison.CurrentCultureIgnoreCase))
{
    await Solver.SolveAll();
}
else
{
    await Solver.Solve(args.Select(arg => (Parsed: uint.TryParse(arg, out var value), Value: value))
                           .Where(arg => arg.Parsed)
                           .Select(arg => arg.Value));
}