namespace MonoApp.Generic;

public interface IITerativeFunction
{
    public NumericList Value { get; set; }
    public NumericList Progress(double delta);
}

public interface IAttractor
{
    public NumericList Value { get; set; }
    public double Strength { get; set; }
    public Func<Attractee, IAttractor, NumericList> AttractorFunction { get; set; }
    public void Force(Attractee attractee, double delta);
}

public class Attractor : IAttractor
{
    public static readonly double s_minimumDistance = 0.01;
    public NumericList Value { get; set; }
    public double Strength { get; set; }
    public Func<Attractee, IAttractor, NumericList> AttractorFunction { get; set; }

    public Attractor(
        NumericList value, 
        double strength, 
        Func<Attractee, IAttractor, NumericList> attractorFunction)
    {
        Value = value;
        Strength = strength;
        AttractorFunction = attractorFunction;
    }

    public static NumericList ForceProportionalToDistance(Attractee attractee, IAttractor attractor)
    {
        var distance = NumericList.Distance(attractee.Value, attractor.Value);
        distance = Math.Min(s_minimumDistance, distance);
        var direction = (attractee.Value - attractor.Value).Normalize();
        var force = direction * attractor.Strength / distance;
        return force;
    }

    public void Force(Attractee attractee, double delta)
    {
        attractee.Value += AttractorFunction(attractee, this) * delta;
    }
}


public class Attractee : IITerativeFunction
{
    public NumericList Value { get; set; }

    public List<IAttractor> Attractors { get; set; }

    public Attractee(NumericList baseValue, List<IAttractor> attractors)
    {
        Value = baseValue;
        Attractors = attractors;
    }

    public NumericList Progress(double delta)
    {
        foreach (var attractor in Attractors)
            attractor.Force(this, delta);

        return Value;
    }
}