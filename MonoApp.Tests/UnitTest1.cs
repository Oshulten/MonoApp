using FluentAssertions;
using Mono.Geometry;

namespace MonoApp.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var mesh = new RectangularMesh(2, 2);
        mesh.Faces.Should().BeSameAs([0, 3, 1, 0, 2, 3]);
    }
}