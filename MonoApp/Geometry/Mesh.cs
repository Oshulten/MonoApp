using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Mono.Geometry;

public class Mesh : VertexCollection
{
    public IEnumerable<int> Faces { get; set; } = [];

    public override void Draw(GraphicsDevice device)
    {
        var x =
            TransformedVertices
            .Select(vertex => new VertexPosition(vertex))
            .ToArray();

        device.DrawUserIndexedPrimitives(
            primitiveType: PrimitiveType.TriangleList,
            vertexData: x,
            vertexOffset: 0,
            numVertices: Count,
            indexData: Faces.ToArray(),
            indexOffset: 0,
            primitiveCount: Faces.Count() / 3,
            vertexDeclaration: VertexPosition.VertexDeclaration);
    }
}