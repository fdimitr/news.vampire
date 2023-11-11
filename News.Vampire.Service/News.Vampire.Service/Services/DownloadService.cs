using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using News.Vampire.Service.Constants;
using News.Vampire.Service.Managers;
using News.Vampire.Service.Managers.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace News.Vampire.Service.Services
{
    public class DownloadService : BackgroundService
    {
        private readonly ILogger<DownloadService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DownloadService(IConfiguration configuration, ILogger<DownloadService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"DownloadService is starting.");

            stoppingToken.Register(() =>
                _logger.LogInformation($" DownloadService background task is stopping."));

            int delayMs = Convert.ToInt32(TimeSpan.FromMinutes(_configuration.GetValue<int>(ConfigKey.DownloadPeriodMinutes)).TotalMilliseconds);
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IDownloadManager _downloadManager = scope.ServiceProvider.GetRequiredService<IDownloadManager>(); ;
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation($"DownloadService task doing background work.");

                    try
                    {
                        await _downloadManager.StartSessionAsync(stoppingToken);
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex, "");
                    }
                    await Task.Delay(delayMs, stoppingToken);
                }
            }
            _logger.LogInformation($"DownloadService background task is stopping.");
        }
    }
}
