using FluentAssertions;
using Mono.Geometry;

namespace MonoApp.Tests;

public class IteratorTests
{
    [Theory]
    [InlineData(3, new float[] { 0f, 0.5f, 1f })]
    [InlineData(6, new float[] { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 1.0f })]
    public void Range(int numberOfValues, float[] expected)
    {
        var range = Iterators.Range(numberOfValues);
        range.Should().ContainInOrder(expected);
    }

    [Theory]
    [InlineData(-1, 1, 3, new float[] { -1, 0, 1 })]
    public void RangeMinMax(float min, float max, int numberOfValues, float[] expected)
    {
        var range = Iterators.Range((min, max), numberOfValues);
        range.Should().ContainInOrder(expected);
    }
}