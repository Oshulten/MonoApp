namespace MonoApp.Generic;

public partial class NumericList(IEnumerable<double> values) : 
    List<double>(values)
{
    public static double Distance(NumericList left, NumericList right) =>
        Math.Sqrt(
            Enumerable.Zip(left, right)
                .Select(pair => Math.Pow(pair.First - pair.Second, 2.0))
                .Aggregate(0.0, (curr, next) => curr + next)
        );

    public double Magnitude() =>
        Math.Sqrt(
            this.Aggregate(0.0, (curr, next) => curr + Math.Pow(next, 2))
        );

    public NumericList Normalize()
    {
        var magnitude = Magnitude();
        if (magnitude == 0)
        {
            return new(
                this.Select(value => 0.0)
            );
        }
        return new(
            this.Select(value => value / magnitude)
        );
    }
}
