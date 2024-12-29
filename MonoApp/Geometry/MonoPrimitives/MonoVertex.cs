using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoApp.Geometry;

public enum VertexProperty
{
    Position,
    Color,
    Normal,
    Texture,
}

public class Vertex
{
    Dictionary<VertexProperty, MonoVector> _data
     = new() {
        { VertexProperty.Position, new([0, 0, 0])},
        { VertexProperty.Color, new([0, 0, 0, 1])},
        { VertexProperty.Normal, new([0, 0, 0])},
        { VertexProperty.Texture, new([0, 0])},
    };

    public Vertex(Dictionary<VertexProperty, MonoVector> data)
    {
        _data = data;
    }

    public Vertex(IEnumerable<double> position)
    {
        _data[VertexProperty.Position] = new(position);
    }

    public Vertex(IEnumerable<double> position, IEnumerable<double> color)
    {
        _data[VertexProperty.Position] = new(position);
        _data[VertexProperty.Color] = new(color);
    }

    public Vertex(IEnumerable<double> position, IEnumerable<double> color, IEnumerable<double> normal)
    {
        _data[VertexProperty.Position] = new(position);
        _data[VertexProperty.Color] = new(color);
        _data[VertexProperty.Normal] = new(normal);
    }

    public Vertex(IEnumerable<double> position, IEnumerable<double> color, IEnumerable<double> normal, IEnumerable<double> texture)
    {
        _data[VertexProperty.Position] = new(position);
        _data[VertexProperty.Color] = new(color);
        _data[VertexProperty.Normal] = new(normal);
        _data[VertexProperty.Texture] = new(texture);
    }

    public Vertex Transform(VertexProperty property, Matrix transformation)
    {
        _data[property].Transform(transformation);
        return this;
    }

    public VertexPositionColorNormalTexture MonoVertex() =>
        new()
        {
            Position = _data[VertexProperty.Position],
            Color = _data[VertexProperty.Color],
            Normal = _data[VertexProperty.Normal],
            TextureCoordinate = _data[VertexProperty.Texture]
        };
}
