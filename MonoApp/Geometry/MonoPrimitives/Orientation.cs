using Microsoft.Xna.Framework;

namespace MonoApp.Geometry;

public class Orientation
{
    public enum Property
    {
        Translation,
        Forward,
        Up,
        Right
    }

    private static readonly double s_derivativeDecay = 0.9;

    public Dictionary<Property, List<MonoVector>> Derivatives = new()
    {
        { Property.Translation, []},
        { Property.Forward, []},
        { Property.Up, []},
        { Property.Right, []},
    };

    public Orientation() : this(
        new([0, 0, 0]), 
        new([1, 0, 0]), 
        new([0, 1, 0]), 
        new([0, 0, 1]))
    {
    }

    public Orientation(MonoVector translation, MonoVector forward, MonoVector right, MonoVector up)
    {
        Derivatives[Property.Translation].Add(translation);
        Derivatives[Property.Forward].Add(forward);
        Derivatives[Property.Up].Add(up);
        Derivatives[Property.Right].Add(right);
    }

    public void TranslateRelatively(MonoVector translation)
    {
        var relativeTranslation = translation.Transform(Matrix);
        Translation = new MonoVector(Translation + relativeTranslation);
    }

    public void Yaw(double angle)
    {
        var rotation = Matrix.CreateFromAxisAngle(Up, (float)angle);
        Forward.Transform(rotation);
        Right.Transform(rotation);
    }   

    public void Roll(double angle)
    {
        var rotation = Matrix.CreateFromAxisAngle(Forward, (float)angle);
        Up.Transform(rotation);
        Right.Transform(rotation);
    }

    public void Pitch(double angle)
    {
        var rotation = Matrix.CreateFromAxisAngle(Matrix.Right, (float)angle);
        Up.Transform(rotation);
        Forward.Transform(rotation);
    }

    public Matrix LookAt()
    {
        return Matrix.CreateLookAt(Translation, new MonoVector(Translation+Forward), Up);
    }

    public MonoVector Translation
    {
        get => Derivatives[Property.Translation][0];
        set => Derivatives[Property.Translation][0] = value;
    }
    public MonoVector Forward
    {
        get => Derivatives[Property.Forward][0];
        set => Derivatives[Property.Forward][0] = value;
    }
    public MonoVector Up
    {
        get => Derivatives[Property.Up][0];
        set => Derivatives[Property.Up][0] = value;
    }

    public MonoVector Right
    {
        get => Derivatives[Property.Right][0];
        set => Derivatives[Property.Right][0] = value;
    }

    public Matrix Matrix
    {
        get
        {
            return Matrix.CreateWorld(Translation, Forward, Up);
        }
    }

    public void Update(TimeSpan delta)
    {
        foreach (var (_, derivatives) in Derivatives)
        {
            for (int i = derivatives.Count; i > 0; i--)
            {
                derivatives[i-1] = new MonoVector(derivatives[i-1] + derivatives[i] * delta.TotalSeconds);
                derivatives[i] = new MonoVector(derivatives[i] * s_derivativeDecay * delta.TotalSeconds);
            }
        }
    }
}
