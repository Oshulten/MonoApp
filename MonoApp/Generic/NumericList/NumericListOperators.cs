using System.Numerics;

namespace MonoApp.Generic;

public partial class NumericList :
    IAdditionOperators<NumericList, double, NumericList>,
    IAdditionOperators<NumericList, NumericList, NumericList>,
    ISubtractionOperators<NumericList, NumericList, NumericList>,
    ISubtractionOperators<NumericList, double, NumericList>,
    IMultiplyOperators<NumericList, NumericList, NumericList>,
    IMultiplyOperators<NumericList, double, NumericList>,
    IDivisionOperators<NumericList, NumericList, NumericList>,
    IDivisionOperators<NumericList, double, NumericList>,
    IUnaryNegationOperators<NumericList, NumericList>,
    IEqualityOperators<NumericList, NumericList, bool>
{
    private enum NumericOperation
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }
    
    public static NumericList operator +(NumericList left, NumericList right) =>
        Operate(NumericOperation.Addition, left, right);

    public static NumericList operator -(NumericList left, NumericList right) =>
        Operate(NumericOperation.Subtraction, left, right);

    public static NumericList operator *(NumericList left, NumericList right) =>
        Operate(NumericOperation.Multiplication, left, right);

    public static NumericList operator /(NumericList left, NumericList right) =>
        Operate(NumericOperation.Multiplication, left, right);

    public static NumericList operator -(NumericList vector) => new(
        from value in vector
        select -value
    );

    public static NumericList operator +(NumericList vector, double scalar) => new(
        from leftValue in vector
        select leftValue + scalar
    );

    public static NumericList operator -(NumericList vector, double scalar) => new(
        from leftValue in vector
        select leftValue - scalar
    );

    public static NumericList operator *(NumericList vector, double scalar) => new(
        from leftValue in vector
        select leftValue * scalar
    );

    public static NumericList operator /(NumericList vector, double scalar) => new(
        from leftValue in vector
        select leftValue / scalar
    );

    public static bool operator ==(NumericList? left, NumericList? right)
    {
        if (left is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(NumericList? left, NumericList? right) =>
        !(left == right);

    private static NumericList Operate(NumericOperation operation, NumericList left, NumericList right)
    {
        var defaultValue =
            operation is NumericOperation.Addition or NumericOperation.Subtraction
                ? 0.0f : 1.0f;

        List<double> results = [];

        for (int i = 0; i < Math.Max(left.Count, right.Count); i++)
        {
            var leftValue = i < left.Count ? left[i] : defaultValue;
            var rightValue = i < right.Count ? right[i] : defaultValue;
            var result = operation switch
            {
                NumericOperation.Addition => leftValue + rightValue,
                NumericOperation.Subtraction => leftValue - rightValue,
                NumericOperation.Multiplication => leftValue * rightValue,
                _ => leftValue / rightValue,
            };
            results.Add(result);
        }

        return new(results);
    }
}

