using Mono.Geometry;

namespace MonoApp.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var mesh = new RectangularMesh(2, 3);
        var doubleRange = Iterators.DoubleRange(1, 3, 1, 3);
        
    }
}