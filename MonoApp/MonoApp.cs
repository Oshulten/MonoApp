using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Extensions.Options;
using MonoApp.Cameras;
using MonoApp.Geometry;
using Microsoft.Extensions.Logging;
using MonoApp.Audio;
using MonoApp.Input;

namespace MonoApp;

public class MonoApp : Game
{
    MonoAppOptions _options;
    GraphicsDeviceManager _graphics;
    ICameraManager _cameraManager;
    IGeometryManager _geometryManager;
    IAudioManager _audioManager;
    IInputManager _inputManager;
    ILogger _logger;
    Effect? _effect;

    public MonoApp(
        IOptions<MonoAppOptions> options,
        IGeometryManager geometryManager,
        IAudioManager audioManager,
        ICameraManager cameraManager,
        IInputManager inputManager,
        ILogger<MonoApp> logger)
    {
        _options = options.Value;
        _graphics = new GraphicsDeviceManager(this);
        _cameraManager = cameraManager;
        _geometryManager = geometryManager;
        _audioManager = audioManager;
        _inputManager = inputManager;
        _logger = logger;

        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        _logger.LogInformation("Initialize");
        _graphics.PreferredBackBufferWidth = _options.Width;
        _graphics.PreferredBackBufferHeight = _options.Height;
        _graphics.IsFullScreen = _options.IsFullscreen;
        _graphics.ApplyChanges();

        _graphics.GraphicsDevice.RasterizerState = new()
        {
            CullMode = CullMode.None,
            FillMode = FillMode.WireFrame
        };

        Window.Title = _options.WindowTitle;

        _cameraManager.Create(
            device: _graphics.GraphicsDevice,
            position: new(0, -0.001f, 5),
            target: Vector3.Zero
        );

        _inputManager.RegisterEvent(Keys.Escape, Input.KeyTrigger.Pressed, () =>
        {
            _logger.LogInformation("Exit");
            Exit();
        });

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _logger.LogInformation("LoadContent");
        _effect = Content.Load<Effect>("effects");
        _effect.CurrentTechnique = _effect.Techniques["ColoredNoShading"];

        _geometryManager.Setup(_graphics.GraphicsDevice);
        _audioManager.Setup();
    }

    protected override void Update(GameTime gameTime)
    {
        _cameraManager.CurrentCamera.Control();
        _geometryManager.Update();
        _audioManager.Update();
        _inputManager.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.WhiteSmoke);

        _cameraManager.CurrentCamera.Apply(_effect!);

        foreach (EffectPass pass in _effect!.CurrentTechnique.Passes)
        {
            pass.Apply();
            _geometryManager.Draw();
        }

        base.Draw(gameTime);
    }
}