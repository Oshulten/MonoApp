using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Mono.Geometry;

public class RectangularMesh : Mesh
{
    public int Width { get; set; }
    public int Height { get; set; }

    public RectangularMesh(int width, int height)
    {
        Width = width;
        Height = height;

        Vertices = Iterators.VectorGrid(Width + 1, Height + 1);

        Faces = Iterators.DoubleRange(0, Width, 0, Height)
            .SelectMany(uv =>
            {
                var u = uv.Item1;
                var v = uv.Item2;
                return new int[] {
                    Index(u, v),
                    Index(u + 1, v + 1),
                    Index(u + 1, v),
                    Index(u, v),
                    Index(u, v + 1),
                    Index(u + 1, v + 1)};
            });
    }

    public int Index(int u, int v)
    {
        if (u > Width)
            throw new ArgumentException($"{nameof(u)}: {u} cannot be greater than {nameof(Width)}: {Width}.", nameof(u));

        if (u < 0)
            throw new ArgumentException($"{nameof(u)}: {u} must be greater or equal to 0.", nameof(u));

        if (v > Height)
            throw new ArgumentException($"{nameof(v)}: {v} cannot be greater than {nameof(Height)}: {Height}.", nameof(v));

        if (v < 0)
            throw new ArgumentException($"{nameof(v)}: {v} must be greater or equal to 0.", nameof(v));

        return v * (Width + 1) + u;
    }

    public static readonly Func<Vector3, Vector3> CylindricalTransform = vertex =>
    {
        float theta = vertex.X * MathHelper.TwoPi;

        return new()
        {
            X = (float)Math.Cos(theta),
            Y = vertex.Y,
            Z = (float)Math.Sin(theta)
        };
    };
}