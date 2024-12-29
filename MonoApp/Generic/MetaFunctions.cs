namespace MonoApp.Generic;

public static class MetaFunctions
{
    public record Point(double X, double Y);
    public static Func<double, double> LinearMultipartFunction(List<Point> points)
    {
        return (t) =>
        {
            if (t <= points[0].X)
                return points[0].Y;

            for (int i = 0; i < points.Count - 1; i++)
            {
                if (!Functions.InRange(t, points[i].X, points[i + 1].X))
                    continue;

                var p = (t - points[i].X) / (points[i + 1].X - points[i].X);
                return (1.0 - p) * points[i].Y + p * points[i + 1].Y;
            }

            return points[^1].Y;
        };
    }

    public static Func<double, double> ADSR(List<Point> points, CancellationToken releaseToken)
    {
        return (t) =>
        {
            if (releaseToken.IsCancellationRequested)
            {
                t -= points[^2].X;
            }
            else if (t > points[^2].X)
            {
                t = points[^2].X;
            }

            if (t <= points[0].X)
                return points[0].Y;

            for (int i = 0; i < points.Count - 1; i++)
            {
                if (!Functions.InRange(t, points[i].X, points[i + 1].X))
                    continue;

                var p = (t - points[i].X) / (points[i + 1].X - points[i].X);
                return (1.0 - p) * points[i].Y + p * points[i + 1].Y;
            }

            return points[^1].Y;
        };
    }
}