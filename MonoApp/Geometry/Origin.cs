using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mono.Geometry;

public class Origin
{
    readonly IEnumerable<VertexCollection> _axis;
    readonly IEnumerable<VertexCollection> _grid;

    public Origin()
    {
        _axis = [
            new Polyline(vertices: [new(0f, 0f, 0f), new(100f, 0f, 0f)])
            {
                Color = Color.Red,
            },
            new Polyline (vertices: [new(0f, 0f, 0f), new(0f, 100f, 0f)])
            {
                Color = Color.Green,
            },
            new Polyline (vertices: [new(0f, 0f, 0f), new(0f, 0f, 100f)])
            {
                Color = Color.Blue,
            }
        ];

        _grid = new Polyline(vertices: [new(0, 0, 0), new(1, 0, 0), new(1, 1, 0), new(0, 1, 0)])
        {
            Color = Color.White,
            IsClosed = true
        }
        .ApplyTransformation<Polyline>(Matrix.CreateTranslation(-10, -10, 0))
        .RectangularArray<Polyline>(new(1, 0, 0), new(0, 1, 0), 20, 20);
    }

    public void Draw(GraphicsDevice device)
    {
        foreach (var axis in _axis)
        {
            axis.Draw(device);
        }

        foreach (var tile in _grid)
        {
            tile.Draw(device);
        }
    }
}