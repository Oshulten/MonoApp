using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;

namespace MonoApp;

public class HostedService(
    ILogger<HostedService> logger,
    IHostApplicationLifetime appLifeTime,
    Game game
) : IHostedService
{
    private readonly ILogger _logger = logger;
    private readonly IHostApplicationLifetime _appLifetime = appLifeTime;
    private readonly Game _game = game;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(() => {
            _game.Run();
        });

        _appLifetime.ApplicationStopping.Register(() => {
            _game.Exit();
        });

        _appLifetime.ApplicationStopped.Register(() => {

        });

        _game.Exiting += (sender, e) => {
            StopAsync(new CancellationToken());
        };

        _logger.LogInformation("StartAsync");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StopAsync");

        _appLifetime.StopApplication();

        return Task.CompletedTask;
    }
}