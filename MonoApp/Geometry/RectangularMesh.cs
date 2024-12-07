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

        Vertices = from uv in Iterators.Grid(Width, Height)
                   select new Vector3(uv.Item1, uv.Item2, 0);

        Faces = Iterators.DoubleRange(0, Height - 1, 0, Width - 1)
            .SelectMany(uv =>
            {
                var u = uv.Item1;
                var v = uv.Item2;
                return new int[] {Index(u, v),
                    Index(u + 1, v + 1),
                    Index(u + 1, v)};
            });
    }

    public int Index(int u, int v)
    {
        if (u >= Width)
            throw new ArgumentException($"{nameof(u)}: {u} must be lesser than {nameof(Width)}: {Width}.", nameof(u));

        if (u < 0)
            throw new ArgumentException($"{nameof(u)}: {u} must be greater or equal to 0.", nameof(u));

        if (v >= Height)
            throw new ArgumentException($"{nameof(v)}: {v} must be lesser than {nameof(Height)}: {Height}.", nameof(v));

        if (v < 0)
            throw new ArgumentException($"{nameof(v)}: {v} must be greater or equal to 0.", nameof(v));

        return v * Width + u;
    }
}