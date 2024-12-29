using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoApp.Generic;

namespace MonoApp.Geometry;

public class VertexCollection(
    PrimitiveType primitiveType,
    IEnumerable<Vertex> vertices,
    IEnumerable<int> indices,
    int primitiveCount)
{
    public PrimitiveType PrimitiveType { get; set; } = primitiveType;
    public List<Vertex> Vertices { get; set; } = vertices.ToList();
    public List<int> Indices { get; set; } = indices.ToList();
    public int PrimitiveCount { get; private set; } = primitiveCount;

    public VertexCollection Transform(VertexProperty property, Matrix transformation)
    {
        foreach (var vertex in Vertices)
        {
            vertex.Transform(property, transformation);
        }

        return this;
    }

    public virtual void Draw(GraphicsDevice device)
    {
        var vertices = Vertices.Select(vertex => vertex.MonoVertex()).ToArray();

        device.DrawUserIndexedPrimitives(
            primitiveType: PrimitiveType,
            vertexData: vertices,
            vertexOffset: 0,
            numVertices: vertices.Length,
            indexData: Indices.ToArray(),
            indexOffset: 0,
            primitiveCount: PrimitiveCount,
            vertexDeclaration: VertexPositionColorNormalTexture.VertexDeclaration);
    }

    public virtual void Draw(Matrix transformation, GraphicsDevice device)
    {
        var vertices = Vertices
            .Select(vertex => vertex
                .Transform(VertexProperty.Position, transformation)
                .MonoVertex())
            .ToArray();
            
        device.DrawUserIndexedPrimitives(
            primitiveType: PrimitiveType,
            vertexData: vertices,
            vertexOffset: 0,
            numVertices: vertices.Length,
            indexData: Indices.ToArray(),
            indexOffset: 0,
            primitiveCount: PrimitiveCount,
            vertexDeclaration: VertexPositionColorNormalTexture.VertexDeclaration);
    }

    public static VertexCollection PolyLine(IEnumerable<Vertex> vertices) => new
    (
        PrimitiveType.LineStrip,
        vertices,
        Iterators.Integral.Range(0, 100),
        vertices.Count() - 1
    );

    public static VertexCollection LineSegments(IEnumerable<Vertex> vertices, IEnumerable<int> indices) => new
    (
        PrimitiveType.LineList,
        vertices,
        indices,
        vertices.Count() / 2
    );

    public static VertexCollection Mesh(IEnumerable<Vertex> vertices, IEnumerable<int> indices) => new
    (
        PrimitiveType.TriangleList,
        vertices,
        indices,
        indices.Count() / 3
    );
}