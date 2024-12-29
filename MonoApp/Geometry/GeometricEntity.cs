using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoApp.Input;

namespace MonoApp.Geometry;

public class GeometricEntity
{
    public Orientation Orientation { get; set; } = new();
    public VertexCollection Geometry { get; set; } = new RectangularMesh(5, 10);

    public void Setup(IInputManager inputManager)
    {
        inputManager.RegisterEvent(Keys.I, KeyTrigger.Pressed, () =>
        {
            Orientation.TranslateRelatively(new([0.1, 0, 0]));
        });

        inputManager.RegisterEvent(Keys.O, KeyTrigger.Pressed, () =>
        {
            Orientation.Yaw(0.1);
        });
    }

    public void Draw(GraphicsDevice device)
    {

        Geometry.Draw(Matrix.Invert(Orientation.Matrix), device);
    }
}