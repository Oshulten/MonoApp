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
        device.DrawUserIndexedPrimitives(
            primitiveType: PrimitiveType.TriangleList,
            vertexData: TransformedEntities
                .Select(vertex => new VertexPosition(vertex))
                .ToArray(),
            vertexOffset: 0,
            numVertices: Count,
            indexData: Faces.ToArray(),
            indexOffset: 0,
            primitiveCount: Faces.Count() / 3,
            vertexDeclaration: VertexPosition.VertexDeclaration);
    }
}