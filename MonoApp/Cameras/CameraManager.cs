using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoApp.Cameras;

public class CameraManager : ICameraManager
{
    private List<Camera> _cameras = [];
    private int _currentCameraIndex = 0;
    public int CurrentCameraIndex
    {
        get => _currentCameraIndex;
        set => _currentCameraIndex = value < _cameras.Count
            ? value
            : 0;
    }

    public Camera Create(GraphicsDevice device, Vector3 position, Vector3 target)
    {
        var camera = new Camera(device.Viewport.AspectRatio, position, target);
        _cameras.Add(camera);
        return camera;
    }

    public Camera CurrentCamera =>
        _cameras[CurrentCameraIndex];
}