using Microsoft.Xna.Framework.Graphics;

namespace MonoApp.Geometry;

public interface IGeometryManager
{
    void Setup(GraphicsDevice graphicsDevice);
    void Update();
    void Draw();
}
