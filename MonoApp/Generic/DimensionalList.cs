using System.Numerics;

namespace MonoApp.Geometry;

public interface IDimensionalList<T>
    where T : INumber<T>
{
    List<T> this[IEnumerable<int> indices] { get; set; }
    List<int> Dimensions { get; init; }
    List<T> Elements { get; }
}

public class DimensionalList<T> : IDimensionalList<T> where T : INumber<T>
{
    private int ElementCount() => Dimensions
        .Aggregate(1, (accumulated, current) => accumulated * current);

    private List<int>? _dimensionWeights;

    private List<int> CalculateDimensionWeights()
    {
        var leftShiftedDimensions = Dimensions[1..];

        _dimensionWeights = [1];
        for (int i = leftShiftedDimensions.Count - 1; i >= 0; i--)
        {
            var newWeight = _dimensionWeights[0] * leftShiftedDimensions[i];
            _dimensionWeights.Insert(0, newWeight);
        }

        return _dimensionWeights;
    }

    public List<int> DimensionWeights =>
        _dimensionWeights ?? CalculateDimensionWeights();

    public DimensionalList(IEnumerable<int> dimensions)
    {
        Dimensions = dimensions.ToList();

        Elements = Enumerable.Range(0, ElementCount())
            .Select(i => default(T)!)
            .ToList();
    }

    public DimensionalList(IEnumerable<int> dimensions, IEnumerable<T> elements)
    {
        Dimensions = dimensions.ToList();

        var elementCount = ElementCount();

        List<T> elementBuilder = elements.Take(elementCount).ToList();

        while (elementBuilder.Count < elementCount)
            elementBuilder.Add(default!);

        Elements = elementBuilder;
    }

    public DimensionalList(IEnumerable<T> elements)
    {
        Dimensions = [elements.Count()];
        Elements = elements.ToList();
    }

    public int StartIndex(IEnumerable<int> indices)
    {
        return Enumerable
            .Zip(indices, DimensionWeights)
            .Select(values => values.First * values.Second)
            .Aggregate(0, (acc, curr) => acc + curr);
    }

    public int ChunkSize(IEnumerable<int> indices)
    {
        return DimensionWeights[indices.Count() - 1];
    }

    public List<T> this[IEnumerable<int> indices]
    {
        get
        {
            var startIndex = StartIndex(indices);
            var chunkSize = ChunkSize(indices);

            return Elements[startIndex..(startIndex + chunkSize)];
        }
        set
        {
            var startIndex = StartIndex(indices);
            var chunkSize = ChunkSize(indices);

            for (int i = startIndex; i < startIndex + chunkSize; i++)
                Elements[i] = value[i - startIndex];
        }
    }

    public List<int> Dimensions { get; init; }
    public List<T> Elements { get; protected set; } = [];

    public static implicit operator List<T>(DimensionalList<T> list) =>
        list.Elements.ToList();
}