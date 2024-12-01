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

    public override void Draw(GraphicsDevice device)
    {
        List<Vector3> vertices = [.. Vertices];

        if (IsClosed)
        {
            vertices.Add(Vertices.First());
        }

        device.DrawUserPrimitives(
            primitiveType: PrimitiveType.LineStrip,
            vertexData: [.. vertices.Select(vertex => new VertexPositionColor(
                position: Vector3.Transform(vertex, Transformation),
                color: Color ))],
            vertexOffset: 0,
            primitiveCount: vertices.Count - 1,
            vertexDeclaration: VertexPositionColor.VertexDeclaration);
    }
}
