using System;
using System.Collections.Generic;
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

        Vertices = PlanarGrid(Width, Height);
        Faces = MakeFaces();
    }

    public static IEnumerable<Vector3> PlanarGrid(int width, int height)
    {
        var vertices = new List<Vector3>();

        foreach (var v in Iterator.Range(width))
        {
            foreach (var u in Iterator.Range(height))
            {
                vertices.Add(new(u, v, 0));
            }
        }

        return vertices;
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

    private IEnumerable<int> MakeFaces()
    {
        var indices = new List<int>();

        foreach (var v in Enumerable.Range(0, Height - 1))
        {
            foreach (var u in Enumerable.Range(0, Width - 1))
            {
                indices.AddRange([
                    Index(u, v),
                    Index(u + 1, v + 1),
                    Index(u + 1, v)
                ]);

                indices.AddRange([
                    Index(u, v),
                    Index(u, v + 1),
                    Index(u + 1, v + 1)
                ]);
            }
        }

        return indices;
    }
}