namespace MonoApp.Generic;

public interface IBinaryTransformable<TSelf>
{
    TSelf Transform(
        TSelf other,
        Func<TSelf, TSelf, TSelf> transformation);

    TSelf TransformInplace(
        TSelf other,
        Func<TSelf, TSelf, TSelf> transformation);
}

public interface IAutoTransformable<TSelf>
{
    TSelf Transform(Func<TSelf, TSelf> transformation);
    TSelf TransformInplace(Func<TSelf, TSelf> transformation);
}

public interface IElementWiseTransformable<TSelf, TElement> where TSelf : IEnumerable<TElement>
{
    TSelf Transform(Func<TElement, TElement> transformation);
    TSelf TransformInplace(Func<TElement, TElement> transformation);
}

public partial class NumericList :
    IBinaryTransformable<NumericList>,
    IAutoTransformable<NumericList>,
    IElementWiseTransformable<NumericList, double>
{
    private void CopyValuesFrom(NumericList other)
    {
        var result = ReturnValuesFrom(other);
        Clear();
        foreach (var v in result) Add(v);
    }

    private NumericList ReturnValuesFrom(NumericList other)
    {
        List<double> result = [];

        for (int i = 0; i < Math.Max(other.Count, Count); i++)
        {
            if (i < other.Count)
            {
                result.Add(other[i]);
                continue;
            }

            result.Add(this[i]);
        }

        return new(result);
    }

    // IBinaryTransformable
    public NumericList Transform(NumericList other, Func<NumericList, NumericList, NumericList> transformation) =>
        ReturnValuesFrom(transformation(this, other));

    public NumericList TransformInplace(NumericList other, Func<NumericList, NumericList, NumericList> transformation)
    {
        CopyValuesFrom(transformation(this, other));
        return this;
    }

    // IAutoTransformable
    public NumericList Transform(Func<NumericList, NumericList> transformation) =>
        ReturnValuesFrom(transformation(this));


    public NumericList TransformInplace(Func<NumericList, NumericList> transformation)
    {
        CopyValuesFrom(transformation(this));
        return this;
    }

    // IElementWiseTransformable
    public NumericList Transform(Func<double, double> transformation) =>
        new(this.Select(value => transformation(value)));

    public NumericList TransformInplace(Func<double, double> transformation)
    {
        ForEach((double value) => transformation(value));
        return this;
    }
}

