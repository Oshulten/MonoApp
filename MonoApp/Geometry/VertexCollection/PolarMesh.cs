using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoApp.Generic;

namespace MonoApp.Geometry;

public class PolarMesh(int spokes, int turns) : VertexCollection(
    primitiveType: PrimitiveType.TriangleList,
    vertices: NormalizedPolarVertices(spokes, turns),
    indices: Faces(spokes, turns),
    primitiveCount: spokes * (2 * turns - 1)
    )
{
    public int Spokes { get; private set; } = spokes;
    public int Turns { get; private set; } = turns;

    public int Index(int turn, int spoke) =>
        turn == 0
            ? 0
            : (turn - 1) * Spokes + (spoke % Spokes) + 1;

    public int Index(double theta, double normalizedRadius)
    {
        throw new NotImplementedException();
    }

    public static int Index(int spokes, int turn, int spoke) =>
        turn == 0
            ? 0
            : (turn - 1) * spokes + (spoke % spokes) + 1;

    public static IEnumerable<Vertex> NormalizedPolarVertices(int spokes, int turns)
    {
        List<Vertex> vertices = [new([0, 0, 0])];
        foreach (int i in Iterators.Integral.Range(1, turns))
        {
            var radius = (1 / turns) * i;
            vertices.AddRange(
                from vertex in Iterators.Decimal.Circle(radius, spokes)
                select new Vertex(position: [.. vertex, 0])
            );
        }
        return vertices;
    }


    public static IEnumerable<int> Faces(int spokes, int turns) =>
        Iterators.Integral.Grid(0, turns + 1, 0, spokes + 1)
            .SelectMany(turnSpoke =>
            {
                var turn = turnSpoke.U;
                var spoke = turnSpoke.V;
                return turn == 0
                    ? new int[] {
                        0,
                        Index(spokes, turn, spoke),
                        Index(spokes, turn, spoke + 1) }
                    : [
                        Index(spokes, turn, spoke),
                        Index(spokes, turn + 1, spoke + 1),
                        Index(spokes, turn, spoke + 1),

                        Index(spokes, turn, spoke),
                        Index(spokes, turn + 1, spoke),
                        Index(spokes, turn + 1, spoke + 1)];
            });
}