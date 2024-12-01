using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Mono.Geometry;

public class PolarMesh : Mesh
{
    public int Spokes { get; set; }

    public int Turns { get; set; }

    public PolarMesh(int spokes, int turns)
    {
        Spokes = spokes;
        Turns = turns;

        Vertices = PolarGrid(Turns, Spokes);
        Faces = MakeFaces();
    }

    public static IEnumerable<Vector3> PolarGrid(int turns, int spokes)
    {
        var vertices = new List<Vector3>() { new(0, 0, 0) };

        foreach (var radius in Iterator.Range((0, 1), turns))
        {
            vertices.AddRange(VectorGenerator.Circle(spokes).Transform(Matrix.CreateScale(radius)));
        }

        return vertices;
    }

    public int Index(int turn, int spoke)
    {
        if (turn == 0) return 0;

        return (turn - 1) * Spokes + (spoke % Spokes) + 1;
    }

    public IEnumerable<int> MakeFaces()
    {
        var indexes = new List<int>();

        for (var turn = 1; turn <= Turns; turn++)
        {
            for (var spoke = 0; spoke < Spokes; spoke++)
            {
                if (turn == 1)
                {
                    indexes.AddRange([0, Index(turn, spoke), Index(turn, spoke + 1)]);
                    continue;
                }

                indexes.AddRange([Index(turn, spoke), Index(turn + 1, spoke + 1), Index(turn, spoke + 1)]);
                indexes.AddRange([Index(turn, spoke), Index(turn + 1, spoke), Index(turn + 1, spoke + 1)]);
            }
        }

        return indexes;
    }

}