using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoApp.Cameras;

public interface ICameraManager
{
    Camera Create(GraphicsDevice device, Vector3 position, Vector3 target);
    Camera CurrentCamera { get; }
    int CurrentCameraIndex { get; set; }
}
