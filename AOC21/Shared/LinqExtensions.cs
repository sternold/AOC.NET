namespace System.Linq
{
    public static class LinqExtensions
    {
        public static double Median(this IEnumerable<long> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            var data = source.OrderBy(n => n).ToArray();
            if (data.Length == 0)
                throw new InvalidOperationException();
            if (data.Length % 2 == 0)
                return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2.0;
            return data[data.Length / 2];
        }
    }
}
