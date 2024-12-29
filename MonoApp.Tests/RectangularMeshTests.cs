namespace MonoApp.Geometry;

public class RectangularMeshTests
{
    public static TheoryData<RectangularMesh, List<int>> Data1 =>
        new() {
            {
                new RectangularMesh(1, 1),
                [0, 3, 1, 0, 2, 3]
            },
            {
                new RectangularMesh(2, 1),
                [0, 4, 1, 0, 3, 4, 1, 5, 2, 1, 4, 5]
            },
            {
                new RectangularMesh(1, 2),
                [0, 3, 1, 0, 2, 3, 2, 5, 3, 2, 4, 5]
            },
        };

    [Theory]
    [MemberData(nameof(Data1))]
    public void Constructs_Indices_Correctly(RectangularMesh mesh, List<int> expectedIndices)
    {
        Assert.Equal(expectedIndices, mesh.Indices);
    }

    public static TheoryData<RectangularMesh, int> Data2 =>
        new() {
            {
                new RectangularMesh(1, 1),
                2
            },
            {
                new RectangularMesh(2, 1),
                4
            },
            {
                new RectangularMesh(1, 2),
                4
            },
        };

    [Theory]
    [MemberData(nameof(Data2))]
    public void Constructs_Primitive_Count_Correctly(RectangularMesh mesh, int expectedPrimitiveCount)
    {
        Assert.Equal(expectedPrimitiveCount, mesh.PrimitiveCount);
    }
}
