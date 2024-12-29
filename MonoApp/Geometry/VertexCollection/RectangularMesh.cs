using Microsoft.Xna.Framework.Graphics;
using MonoApp.Generic;

namespace MonoApp.Geometry;

public class RectangularMesh(int width, int height) : VertexCollection(
    primitiveType: PrimitiveType.TriangleList,
    vertices: NormalizedRectangularGrid(width, height),
    indices: Faces(width, height),
    primitiveCount: width * height * 2
    )
{
    public int Width { get; private set; } = width;
    public int Height { get; private set; } = height;

    private int Index(int u, int v) =>
        v * (Width + 1) + u;

    private int Index(double u, double v) =>
        throw new NotImplementedException();

    private static int Index(int width, int u, int v) =>
        v * (width + 1) + u;

    public static List<Vertex> NormalizedRectangularGrid(int width, int height) =>
        Iterators.Decimal.Grid(width + 1, height + 1)
            .Select(position => new Vertex(position: [position.U, position.V, 0]))
            .ToList();

    public static List<int> Faces(int width, int height) =>
        Iterators.Integral.Grid(0, width, 0, height)
            .SelectMany(position =>
                new List<int>()
                {
                    Index(width, position.U, position.V),
                    Index(width, position.U + 1, position.V + 1),
                    Index(width, position.U + 1, position.V),
                    Index(width, position.U, position.V),
                    Index(width, position.U, position.V + 1),
                    Index(width, position.U + 1, position.V + 1)
                }
            )
            .ToList();
}