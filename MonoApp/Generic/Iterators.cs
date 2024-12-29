using Microsoft.Xna.Framework;

namespace MonoApp.Generic;

/*
    A collection of iterators to simplify LINQ usage.
*/
public static class Iterators
{
    public static class Integral
    {
        /*
            Iterates from start in steps of 1.
            Yields <count> times.
        */
        public static IEnumerable<int> Range(int start, int count)
        {
            for (var i = start; i < start + count; i++)
                yield return i;
        }

        /*
            Iterates from (uStart, vStart) in sequential steps.
            Yields <uCount>*<vCount> times (the Cartesian set of both ranges)
        */
        public static IEnumerable<(int U, int V)> Grid(int uStart, int uCount, int vStart, int vCount)
        {
            return from u in Range(uStart, uCount)
                   from v in Range(vStart, vCount)
                   select (u, v);
        }
    }

    public static class Decimal
    {
        /*
            Iterates from 0 to 1 (inclusive both ends).
            Yields >count> times.
        */
        public static IEnumerable<double> Range(int count)
        {
            var delta = 1.0 / (count - 1.0);
            return from i in Integral.Range(0, count)
                   select delta * i;
        }

        /*
            Iterates from min to max (inclusive both ends).
            Yields <count> times.
        */
        public static IEnumerable<double> Range(double min, double max, int count)
        {
            var interval = max - min;
            var delta = interval / (count - 1);

            return from i in Integral.Range(0, count)
                   select min + delta * i;
        }

        public static IEnumerable<double> GeometricSeries(double baseValue, double proportion, int count) =>
            Integral.Range(0, count)
                .Select(n => baseValue * Math.Pow(proportion, n));


        // ? Should this enumerate with u as inner? Integral.Grid has v as inner
        public static IEnumerable<(double U, double V)> Grid(int uValues, int vValues) =>
            from v in Range(vValues)
            from u in Range(uValues)
            select (u, v);

        // ? Should this enumerate with u as inner? Integral.Grid has v as inner
        public static IEnumerable<(double U, double V)> Grid(
        double uMin, double uMax, int uValues,
        double vMin, double vMax, int vValues) =>
            from v in Range(vMin, vMax, vValues)
            from u in Range(uMin, uMax, uValues)
            select (u, v);

        public static IEnumerable<IEnumerable<double>> Circle(double radius, int segments, double thetaOffset = 0) =>
            from theta in Decimal.Range(0, 2 * MathHelper.Pi, segments)
            select new List<double>() {
                radius * Math.Cos(-theta + thetaOffset),
                radius * Math.Sin(-theta + thetaOffset)
            };
    }
}

public static class IEnumerableExtensions
{
    /*
        Applies a matrix transform on each vector in an IEnumerable.
    */
    public static IEnumerable<Vector3> Transform(this IEnumerable<Vector3> vectors, Matrix matrix) =>
        from vector in vectors
        select Vector3.Transform(vector, matrix);

    /*
        Executes an action on each entity if an IEnumerable.
        Does not mutate the IEnumerable.
    */
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var entity in enumerable)
            action(entity);
    }
}