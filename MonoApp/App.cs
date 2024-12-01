using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mono.Geometry;

namespace Mono;

internal record struct WindowSettings(
    int Width,
    int Height,
    bool IsFullscreen,
    string Title,
    Color Background);

public class App : Game
{
    static WindowSettings s_windowSettings = new(
        Width: 800,
        Height: 500,
        IsFullscreen: false,
        Title: "Oscar's 3D Adventures",
        Background: Color.DarkSlateBlue);

    GraphicsDeviceManager _graphics;
    GraphicsDevice Device => _graphics.GraphicsDevice;
    Effect? _effect;
    Camera? _camera;

    List<VertexCollection> _geometry = [];

    public App()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = s_windowSettings.Width;
        _graphics.PreferredBackBufferHeight = s_windowSettings.Height;
        _graphics.IsFullScreen = s_windowSettings.IsFullscreen;
        _graphics.ApplyChanges();

        Window.Title = s_windowSettings.Title;

        _camera = new(Device, new(0, -50, 50), Vector3.Zero);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _effect = Content.Load<Effect>("effects");

        _geometry.Add(new RectangularMesh(10, 5));
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _camera?.Control();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        if (_camera is null || _effect is null)
            return;

        var mesh = new RectangularMesh(10, 5).Transform<Mesh>(Matrix.CreateScale(100));

        var origin = new Origin();

        GraphicsDevice.Clear(s_windowSettings.Background);

        Device.RasterizerState = new()
        {
            CullMode = CullMode.None,
            FillMode = FillMode.WireFrame
        };

        _effect.CurrentTechnique = _effect.Techniques["ColoredNoShading"];

        _camera.Apply(effect: _effect);

        foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
        {
            pass.Apply();

            foreach (var geometry in _geometry)
                geometry.Draw(Device);

            origin.Draw(Device);
        }

        base.Draw(gameTime);
    }
}