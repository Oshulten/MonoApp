using System.Collections.Generic;
using System.Linq;
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

        Vertices = from radius in Iterators.Range((0, 1), Turns)
                   from vertex in VectorGenerator.Circle(Spokes)
                       .Transform(Matrix.CreateScale(radius))
                   select vertex;

        Faces = Iterators.DoubleRange(1, Turns, 0, Spokes)
            .SelectMany(turnSpoke =>
            {
                var turn = turnSpoke.Item1;
                var spoke = turnSpoke.Item2;
                return turn == 1
                    ? new int[] {
                        0,
                        Index(turn, spoke),
                        Index(turn, spoke + 1) }
                    : [
                        Index(turn, spoke),
                        Index(turn + 1, spoke + 1),
                        Index(turn, spoke + 1),

                        Index(turn, spoke),
                        Index(turn + 1, spoke),
                        Index(turn + 1, spoke + 1)];
            });
    }

    public int Index(int turn, int spoke) =>
        turn == 0
            ? 0
            : (turn - 1) * Spokes + (spoke % Spokes) + 1;
}