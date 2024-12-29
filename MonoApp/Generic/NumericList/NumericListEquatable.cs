namespace MonoApp.Generic;

public partial class NumericList : IEquatable<NumericList>
{
    public static readonly double Epsilon = 0.001;
    
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object? obj) =>
        Equals(obj as NumericList);

    public bool Equals(NumericList? other)
    {
        if (other is null) return false;

        if (ReferenceEquals(this, other)) return true;

        if (Count != other.Count) return false;

        foreach (var (left, right) in Enumerable.Zip(this, other))
        {
            if (Math.Abs(left - right) > Epsilon) return false;
        }

        return true;
    }
}

