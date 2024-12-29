using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Xna.Framework;
using MonoApp;
using MonoApp.Cameras;
using MonoApp.Geometry;
using MonoApp.Audio;
using MonoApp.Input;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{
    services.AddHostedService<HostedService>();
    services.AddSingleton<Game, MonoApp.MonoApp>();

    services.AddSingleton<IGeometryManager, GeometryManager>();
    services.AddSingleton<IAudioManager, AudioManager>();
    services.AddSingleton<ICameraManager, CameraManager>();
    services.AddSingleton<IInputManager, InputManager>();
});

builder.Build().Run();


