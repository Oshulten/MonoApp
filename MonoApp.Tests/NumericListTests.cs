namespace MonoApp.Generic;

public class NumericListTests
{
    public class Magnitude
    {
        [Theory]
        [InlineData(new double[] { 1, 1, 1 }, 1.73)]
        [InlineData(new double[] { 0 }, 0)]
        [InlineData(new double[] { 1, 0 }, 1)]
        [InlineData(new double[] { 2, 2 }, 2.82)]
        public void Should_Yield_Correct_Value(double[] values, double expectedMagnitude)
        {
            Assert.InRange(
                new NumericList(values).Magnitude(), 
                expectedMagnitude-0.01, 
                expectedMagnitude+0.01);
        }
    }

    public class Distance
    {
        [Theory]
        [InlineData(new double[] { 1, 1, 1 }, new double[] { 1, 1, 1 }, 0)]
        [InlineData(new double[] { 0 }, new double[] { 1 }, 1)]
        [InlineData(new double[] { 1, 1 }, new double[] { 0 },  1)]
        public void Should_Yield_Correct_Value(double[] valuesLeft, double[] valuesRight, double expectedDistance)
        {
            Assert.InRange(
                NumericList.Distance(new(valuesLeft), new(valuesRight)), 
                expectedDistance-0.01, 
                expectedDistance+0.01);
        }
    }

    public class Normalize
    {
        [Theory]
        [InlineData(new double[] { 1, 1, 1 })]
        [InlineData(new double[] { 1, 0 })]
        [InlineData(new double[] { 2, 2 })]
        // Does not work with zero vectors
        public void Magnitude_Should_Equal_1(double[] values)
        {
            var normalizedVector = new NumericList(values).Normalize().Magnitude();

            Assert.InRange(
                new NumericList(values).Normalize().Magnitude(), 
                1-0.01, 
                1+0.01);
        }
    }

    public class Equatable
    {
        public static TheoryData<double[], double[], bool> EqualityData =>
            new() {
                { [1, 0],   [1, 0],                         true },
                // An absolute difference within NumericList.Epsilon results in true
                { [1],      [1 + NumericList.Epsilon  * 0.5],    true },
                { [1],      [1 - NumericList.Epsilon * 0.5],     true },
                { [0, 0],   [1, 0],                         false },
                // Non-matching lengths results in false
                { [],       [1, 0],                         false },
                { [1],      [],                             false },
            };

        [Theory]
        [MemberData(nameof(EqualityData))]
        public void Equality(double[] left, double[] right, bool expected)
        {
            var vectorLeft = new NumericList(left);
            var vectorRight = new NumericList(right);

            var actual = Equals(vectorLeft, vectorRight);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(EqualityData))]
        public void EqualityOperator(double[] left, double[] right, bool expected)
        {
            var vectorLeft = new NumericList(left);
            var vectorRight = new NumericList(right);

            var actual = vectorLeft == vectorRight;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(EqualityData))]
        public void NonEqualityOperator(double[] left, double[] right, bool expected)
        {
            var vectorLeft = new NumericList(left);
            var vectorRight = new NumericList(right);

            var actual = vectorLeft != vectorRight;

            Assert.Equal(expected, !actual);
        }
    }

    public class ScalarOperations
    {
        public static TheoryData<NumericList, double, NumericList> AdditionData =>
            new() {
                { new([]),      1,    new([]) },
                { new([0]),     1,    new([1]) },
                { new([0, 0]),  1,    new([1, 1]) },
            };

        [Theory]
        [MemberData(nameof(AdditionData))]
        public void Addition(NumericList left, double scalar, NumericList expected)
        {
            var result = left + scalar;

            Assert.Equal(expected, result);
        }

        public static TheoryData<NumericList, double, NumericList> SubtractionData =>
            new() {
                { new([]),      1,    new([]) },
                { new([0]),     1,    new([-1]) },
                { new([0, 0]),  1,    new([-1, -1]) },
            };

        [Theory]
        [MemberData(nameof(SubtractionData))]
        public void Subtraction(NumericList left, double scalar, NumericList expected)
        {
            var result = left - scalar;

            Assert.Equal(expected, result);
        }

        public static TheoryData<NumericList, double, NumericList> MultiplicationData =>
            new() {
                { new([]),      0,    new([]) },
                { new([1]),     0,    new([0]) },
                { new([1, 1]),  0,    new([0, 0]) },
                { new([]),      1,    new([]) },
                { new([1]),     1,    new([1]) },
                { new([1, 1]),  1,    new([1, 1]) },
            };

        [Theory]
        [MemberData(nameof(MultiplicationData))]
        public void Multiplication(NumericList left, double scalar, NumericList expected)
        {
            var result = left * scalar;

            Assert.Equal(expected, result);
        }

        public static TheoryData<NumericList, double, NumericList> DivisionData =>
            new() {
                { new([]),      0,    new([]) },
                { new([1]),     0,    new([double.PositiveInfinity]) },
                { new([1, 1]),  0,    new([double.PositiveInfinity, double.PositiveInfinity]) },
                { new([]),      1,    new([]) },
                { new([1]),     1,    new([1]) },
                { new([1, 1]),  1,    new([1, 1]) },
            };

        [Theory]
        [MemberData(nameof(DivisionData))]
        public void Division(NumericList left, double scalar, NumericList expected)
        {
            var result = left / scalar;

            Assert.Equal(expected, result);
        }
    }


    public class BinaryOperations
    {
        public static TheoryData<NumericList, NumericList, NumericList> AdditionData =>
            new() {
                { new([0.0]),      new([0.0]),    new([0.0]) },
                { new([0.0]),      new([1.0]),    new([1.0]) },
                // Shorter vector is lengthened with default value of 0.0 to match longer vector
                { new([]),          new([1.0]),    new([1.0]) },
                { new([1.0]),      new([]),        new([1.0]) },
                // Two empty vectors result in another empty vector
                { new([]),          new([]),        new([]) },
            };

        [Theory]
        [MemberData(nameof(AdditionData))]
        public void Addition(NumericList left, NumericList right, NumericList expected)
        {
            var result = left + right;

            Assert.Equal(expected, result);
        }

        public static TheoryData<NumericList, NumericList, NumericList> SubtractionData =>
            new() {
                { new([0.0]),      new([0.0]),    new([0.0]) },
                { new([0.0]),      new([1.0]),    new([-1.0]) },
                // Shorter vector is lengthened with default value of 0.0 to match longer vector
                { new([]),          new([1.0]),    new([-1.0]) },
                { new([1.0]),      new([]),        new([1.0]) },
                // Two empty vectors result in another empty vector
                { new([]),          new([]),        new([]) },
            };

        [Theory]
        [MemberData(nameof(SubtractionData))]
        public void Subtraction(NumericList left, NumericList right, NumericList expected)
        {
            var result = left - right;

            Assert.Equal(expected, result);
        }

        public static TheoryData<NumericList, NumericList, NumericList> MultiplicationData =>
            new() {
                { new([0.0]),      new([0.0]),    new([0.0]) },
                { new([0.0]),      new([1.0]),    new([0.0]) },
                // Shorter vector is lengthened with default value of 1.0 to match longer vector
                { new([]),          new([1.0]),    new([1.0]) },
                { new([1.0]),      new([]),        new([1.0]) },
                // Two empty vectors result in another empty vector
                { new([]),          new([]),        new([]) },
            };

        [Theory]
        [MemberData(nameof(MultiplicationData))]
        public void Multiplication(NumericList left, NumericList right, NumericList expected)
        {
            var result = left * right;

            Assert.Equal(expected, result);
        }

        public static TheoryData<NumericList, NumericList, NumericList> DivisionData =>
            new() {
                { new([0]),      new([0]),    new([0]) },
                { new([0]),      new([1]),    new([0]) },
                // Shorter vector is lengthened with default value of 1.0 to match longer vector
                { new([]),       new([1]),    new([1]) },
                { new([1]),      new([]),     new([1]) },
                // Two empty vectors result in another empty vector
                { new([]),       new([]),     new([]) },
            };

        [Theory]
        [MemberData(nameof(DivisionData))]
        public void VectorVectorDivision(NumericList left, NumericList right, NumericList expected)
        {
            var result = left / right;

            Assert.Equal(expected, result);

            return;

        }
    }

    public class Transformable
    {
        public static TheoryData<NumericList, Func<NumericList, NumericList>, NumericList> AutoTransformData =>
            new() {
                { new([0]),         (NumericList list) => list + 1,    new([1]) },
                { new([]),          (NumericList list) => list + 1,    new([]) },
                { new([1, 2, 3]),   (NumericList list) => list * 2,    new([2, 4, 6]) },
                // { new([1, 2, 3]),   (NumericList list) => new([0]),    new([0]) },
            };

        [Theory]
        [MemberData(nameof(AutoTransformData))]
        public void AutoTransformInplace(NumericList list, Func<NumericList, NumericList> transformation, NumericList expected)
        {
            list.TransformInplace(transformation);

            Assert.Equal(expected, list);
        }

        [Theory]
        [MemberData(nameof(AutoTransformData))]
        public void AutoTransform(NumericList list, Func<NumericList, NumericList> transformation, NumericList expected)
        {
            var result = list.Transform(transformation);

            Assert.Equal(expected, result);
        }

        public static TheoryData<NumericList, NumericList, Func<NumericList, NumericList, NumericList>, NumericList> BinaryTransformData =>
            new() {
                { 
                    new([0]),  
                    new([1]), 
                    (NumericList left, NumericList right) => left,    
                    new([0]) 
                },
                { 
                    new([1]),  
                    new([2]), 
                    (NumericList left, NumericList right) => left + right,    
                    new([3]) 
                },
                {
                    new([0, 0, 0]),
                    new([1]),
                    (NumericList left, NumericList right) => left + right,
                    new([1, 0, 0])
                },
                { 
                    new([1, 2, 3]),  
                    new([1, 1, 1]), 
                    (NumericList left, NumericList right) => left.Transform((double v) => v * 2) + right,    
                    new([3, 5, 7]) 
                },
            };

        [Theory]
        [MemberData(nameof(BinaryTransformData))]
        public void BinaryTransformInplace(
            NumericList left,
            NumericList right,
            Func<NumericList, NumericList, NumericList> transformation,
            NumericList expected)
        {
            left.TransformInplace(right, transformation);

            Assert.Equal(expected, left);
        }

        [Theory]
        [MemberData(nameof(BinaryTransformData))]
        public void BinaryTransform(
            NumericList left, 
            NumericList right, 
            Func<NumericList, NumericList, NumericList> transformation, 
            NumericList expected)
        {
            var result = left.Transform(right, transformation);

            Assert.Equal(expected, result);
        }
    }
}