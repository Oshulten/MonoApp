using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mono.Geometry;

public class Origin
{
    readonly IEnumerable<Polyline> _axis;
    readonly List<Polyline> _grid;

    public Origin()
    {
        _axis = [
            new Polyline()
            {
                Color = Color.Red,
                Vertices = [new(0f, 0f, 0f), new(100f, 0f, 0f)]
            },
            new ()
            {
                Color = Color.Green,
                Vertices = [new(0f, 0f, 0f), new(0f, 100f, 0f)]
            },
            new ()
            {
                Color = Color.Blue,
                Vertices = [new(0f, 0f, 0f), new(0f, 0f, 100f)]
            }
        ];

        _grid = new Polyline()
        {
            Vertices = [new(0, 0, 0), new(1, 0, 0), new(1, 1, 0), new(0, 1, 0)],
            Color = Color.White,
            IsClosed = true
        }.RectangularArray<Polyline>(new(1, 0, 0), new(0, 1, 0), 20, 20).ToList();

        for (var i = 0; i < 400; i++)
        {
            _grid[i].Transformation *= Matrix.CreateTranslation(-10, -10, 0);
        }
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