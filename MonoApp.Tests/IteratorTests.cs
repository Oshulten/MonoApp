using MonoApp.Generic;
using FluentAssertions;

namespace MonoApp.Tests;

public class IteratorsTest
{
    public class IntegralTests
    {
        [Theory]
        [InlineData(0, 3, new[] { 0, 1, 2 })]
        [InlineData(0, 0, new int[] { })]
        [InlineData(0, 1, new int[] { 0 })]
        [InlineData(-5, 1, new[] { -5 })]
        public void Range(int start, int count, IEnumerable<int> expected)
        {
            Iterators.Integral.Range(start, count)
                .Should()
                .ContainInConsecutiveOrder(expected);
        }

        public static IEnumerable<object[]> GridTestData()
        {
            yield return new object[] { 0, 1, 0, 1, new (int, int)[] { (0, 0) } };
            yield return new object[] { 0, 3, 0, 2, new (int, int)[] {
                (0, 0),
                (0, 1),
                (1, 0),
                (1, 1),
                (2, 0),
                (2, 1),
            }};
        }

        [Theory]
        [MemberData(nameof(GridTestData))]
        public void Grid(int uStart, int uCount, int vStart, int vCount, IEnumerable<(int, int)> expected)
        {
            Iterators.Integral.Grid(uStart, uCount, vStart, vCount)
                .Should()
                .ContainInConsecutiveOrder(expected);
        }
    }

    public class DecimalTests
    {
        [Theory]
        [InlineData(3, new[] { 0, 0.5, 1 })]
        [InlineData(0, new double[] { })]
        public void RangeNormalized(int count, IEnumerable<double> expected)
        {
            Iterators.Decimal.Range(count)
                .Should()
                .ContainInConsecutiveOrder(expected);
        }

        [Theory]
        [InlineData(0, 1, 3, new[] { 0, 0.5, 1 })]
        [InlineData(-1, 1, 3, new double[] { -1, 0, 1 })]
        public void RangeMinMax(double min, double max, int count, IEnumerable<double> expected)
        {
            Iterators.Decimal.Range(min, max, count)
                .Should()
                .ContainInConsecutiveOrder(expected);
        }

        public static IEnumerable<object[]> GridNormalizedTestData()
        {
            yield return new object[] { 3, 2, new (double, double)[] {
                (0f, 0f),
                (0.5f, 0f),
                (1.0f, 0f),
                (0f, 1f),
                (0.5f, 1f),
                (1.0f, 1f),
            }};
        }

        [Theory]
        [MemberData(nameof(GridNormalizedTestData))]
        public void GridNormalized(int uCount, int vCount, IEnumerable<(double, double)> expected)
        {
            Iterators.Decimal.Grid(uCount, vCount)
                .Should()
                .ContainInConsecutiveOrder(expected);
        }


        public static TheoryData<int, int, int, int, int, int, IEnumerable<(double, double)>> GridMinMaxTestData =>
        new()
        {
            { 0, 1, 2, 0, 1, 2, new (double, double)[] { (0f, 0f), (1.0f, 0f), (0f, 1f), (1.0f, 1f) } },
            { 0, 1, 3, 0, 1, 2, new (double, double)[] { (0f, 0f), (0.5f, 0f), (1.0f, 0f), (0f, 1f), (0.5f, 1f), (1.0f, 1f) } }
        };

        [Theory]
        [MemberData(nameof(GridMinMaxTestData))]
        public void GridMinMax(int uMin, int uMax, int uCount, int vMin, int vMax, int vCount, IEnumerable<(double, double)> expected)
        {
            Iterators.Decimal.Grid(uMin, uMax, uCount, vMin, vMax, vCount)
                .Should()
                .ContainInConsecutiveOrder(expected);
        }
    }
}
