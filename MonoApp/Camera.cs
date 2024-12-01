using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mono;

public class MovableMatrix
{
    public MovableMatrix(Vector3 position, Vector3 forward, Vector3 up)
    {
        Matrix = Matrix.CreateWorld(position, forward, up);
    }

    public MovableMatrix(Vector3 position, Vector3 target)
    {
        Matrix = Matrix.Invert(Matrix.CreateLookAt(position, target, Vector3.UnitZ));
    }

    protected Matrix Matrix { get; set; }
    public Vector3 Position => Matrix.Translation;
    public Vector3 Up => Matrix.Up;
    public Vector3 Forward => Matrix.Forward;
    public Vector3 Right => Matrix.Right;

    public Matrix TranslationAlongAxis(Vector3 relativeTranslation)
    {
        var forwardTranslation = Forward * relativeTranslation.X;
        var rightTranslation = Right * relativeTranslation.Y;
        var upTranslation = Up * relativeTranslation.Z;

        return Matrix.CreateTranslation(forwardTranslation + rightTranslation + upTranslation);
    }
}

public class Camera(GraphicsDevice device, Vector3 position, Vector3 target)
: MovableMatrix(position, target)
{
    Matrix _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            fieldOfView: MathHelper.PiOver4,
            aspectRatio: device.Viewport.AspectRatio,
            nearPlaneDistance: 1.0f,
            farPlaneDistance: 300
        );

    public Vector3 Target { get; set; } = target;

    public void Apply(Effect effect)
    {
        var viewMatrix = Matrix.Invert(Matrix);

        effect.Parameters["xView"].SetValue(viewMatrix);
        effect.Parameters["xProjection"].SetValue(_projectionMatrix);
        effect.Parameters["xWorld"].SetValue(Matrix.Identity);
    }

    public void Control()
    {
        float zRotation = 0;
        var directionalTranslation = Vector3.Zero;

        foreach (var key in Keyboard.GetState().GetPressedKeys())
        {
            zRotation = key switch
            {
                Keys.E => 0.1f,
                Keys.Q => -0.1f,
                _ => 0.0f
            };

            directionalTranslation += key switch
            {
                Keys.W => new(0.5f, 0, 0),
                Keys.S => new(-0.5f, 0, 0),
                Keys.D => new(0, 0.5f, 0),
                Keys.A => new(0, -0.5f, 0),
                Keys.R => new(0, 0, 0.5f),
                Keys.F => new(0, 0, -0.5f),
                _ => Vector3.Zero
            };
        }

        Matrix *= Matrix.CreateRotationZ(zRotation);
        Matrix *= TranslationAlongAxis(directionalTranslation);
        Target = Vector3.Transform(Target, TranslationAlongAxis(directionalTranslation));
        Matrix = Matrix.Invert(Matrix.CreateLookAt(Matrix.Translation, Target, Vector3.UnitZ));
    }

    public Matrix CreateBillboard(Vector3 position)
    {
        return Matrix.CreateBillboard(position, Matrix.Translation, Matrix.Up, Matrix.Forward);
    }
}