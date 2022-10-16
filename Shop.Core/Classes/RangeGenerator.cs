

namespace Shop.Core.Classes
{
    public static class RangeGenerator
    {
        public static IEnumerable<double> DoubleRange(double start, double end, double step)
        {
            for (double i = start; i <= end; i += step)
            {
                yield return i;
            }
        }
        public static IEnumerable<int> IntRange(int start, int end, int step)
        {
            for (int i = start; i <= end; i += step)
            {
                yield return i;
            }
        }
    }
}
