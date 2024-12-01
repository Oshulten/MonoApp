using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mono.Geometry;

public class Mesh : VertexCollection
{
    public IEnumerable<int> Faces { get; set; } = [];

    public override void Draw(GraphicsDevice device)
    {
        device.DrawUserIndexedPrimitives(
            primitiveType: PrimitiveType.TriangleList,
            vertexData: Vertices.Select(vertex => new VertexPositionColor(vertex, Color.Black)).ToArray(),
            vertexOffset: 0,
            numVertices: Vertices.Count(),
            indexData: Faces.ToArray(),
            indexOffset: 0,
            primitiveCount: Faces.Count() / 3,
            vertexDeclaration: VertexPositionColor.VertexDeclaration);
    }
}