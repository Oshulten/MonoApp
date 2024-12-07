using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mono.Geometry;

public class Polyline : VertexCollection
{
    public bool IsClosed { get; init; } = false;
    public Color Color { get; set; } = Color.Black;

    public Polyline(IEnumerable<Vector3> vertices)
    {
        Vertices = vertices;
    }

    public override void Draw(GraphicsDevice device)
    {
        List<Vector3> vertices = [.. TransformedVertices];

        if (IsClosed)
        {
            vertices.Add(vertices.First());
        }

        var vertexData = vertices.Select(vertex => new VertexPositionColor(
            position: vertex,
            color: Color))
            .ToArray();

        device.DrawUserPrimitives(
            primitiveType: PrimitiveType.LineStrip,
            vertexData: vertexData,
            vertexOffset: 0,
            primitiveCount: vertexData.Count() - 1,
            vertexDeclaration: VertexPositionColor.VertexDeclaration);
    }
}
