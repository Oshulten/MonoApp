using MonoApp.Generic;
using FluentAssertions;

namespace MonoApp.Generic;

public class FunctionTests
{
    public class CycleInRange
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(0.5, 0.5)]
        [InlineData(1, 0)]
        public void Should_Give_Correct_Values_1(double t, double expectedY)
        {
            var result = Functions.CycleInRange(t, 0, 1);

            Assert.InRange(result, expectedY-0.01, expectedY+0.01);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(0.5, 1.5)]
        [InlineData(1, 1)]
        [InlineData(1.5, 1.5)]
        [InlineData(2, 1)]
        [InlineData(2.5, 1.5)]
        [InlineData(3, 1)]
        public void Should_Give_Correct_Values_2(double t, double expectedY)
        {
            var result = Functions.CycleInRange(t, 1, 2);

            Assert.InRange(result, expectedY-0.01, expectedY+0.01);
        }
    }
}
