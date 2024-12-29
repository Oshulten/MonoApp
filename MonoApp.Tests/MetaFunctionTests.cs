using MonoApp.Generic;
using FluentAssertions;

namespace MonoApp.Generic;

public class MetaFunctionTests
{
    public static double Epsilon = 0.0001;
    public class LinearMultipartFunction
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, 0)]
        [InlineData(1, 0)]
        public void Should_Give_Correct_Values_1(double t, double expectedY)
        {
            var func = MetaFunctions.LinearMultipartFunction([new(0, 0)]);
            var result = func(t);

            Assert.InRange(result, expectedY - Epsilon, expectedY + Epsilon);
        }

        [Theory]
        [InlineData(-2, 1)]
        [InlineData(-1.5, 1)]
        [InlineData(-1, 1)]
        [InlineData(-0.5, 0.5)]
        [InlineData(0, 0)]
        [InlineData(0.5, 0.5)]
        [InlineData(1, 1)]
        [InlineData(1.5, 1)]
        public void Should_Give_Correct_Values_2(double t, double expectedY)
        {
            var func = MetaFunctions.LinearMultipartFunction([new(-1, 1), new(0, 0), new(1, 1)]);
            var result = func(t);

            Assert.InRange(result, expectedY - Epsilon, expectedY + Epsilon);
        }
    }
}
