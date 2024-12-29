using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoApp.Generic;
using MonoApp.Input;

namespace MonoApp.Geometry;

public class GeometryManager(IInputManager inputManager) : IGeometryManager
{
    private IInputManager _inputManager = inputManager;
    private List<VertexCollection> _geometry = [];
    private List<GeometricEntity> _entities = [];
    private GraphicsDevice? _graphicsDevice;

    private static IEnumerable<VertexCollection> ManyCurves(int count, int resolution)
    {
        var polylines = new List<VertexCollection>();
        foreach (var j in Iterators.Integral.Range(0, count))
        {
            var vertices = new List<Vertex>();
            foreach (var i in Iterators.Decimal.Range(resolution))
            {
                vertices.Add(new Vertex(
                    position: [i * Math.Cos(i * 2 * Math.PI), i * Math.Sin(i * 2 * Math.PI), i],
                    color: [0, 0, 0, 1])
                );
            }
            polylines.Add(VertexCollection.PolyLine(vertices));
        }
        return polylines;
    }

    public void Setup(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;

        // _geometry.Add(
        //     new RectangularMesh(5, 3)
        // );

        // _entities.Add(
        //     new GeometricEntity()
        // );

        _entities.ForEach(entity => entity.Setup(_inputManager));
    }

    public void Draw()
    {
        ArgumentNullException.ThrowIfNull(_graphicsDevice);

        _geometry.ForEach(geometry => geometry.Draw(_graphicsDevice));
        _entities.ForEach(entity => entity.Draw(_graphicsDevice));
    }

    public void Update()
    {
    }
}