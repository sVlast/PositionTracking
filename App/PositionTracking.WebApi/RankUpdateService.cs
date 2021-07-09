using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PositionTracking.Data;
using PositionTracking.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PositionTracking.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace PositionTracking.WebApi
{
    public class RankUpdateService : IHostedService, IDisposable
    {
        private readonly ILogger<RankUpdateService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly Timer _timer;
        private readonly TimeSpan _updateTime;

        public RankUpdateService(ILogger<RankUpdateService> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _updateTime = configuration.GetValue<TimeSpan>("Settings:RankUpdateTimeUTC");
            _timer = new Timer(OnTimerInterval);
        }


        private void OnTimerInterval(object state)
        {
            try
            {
                _logger.LogInformation("Rank update started.");
                //scope otvara ApplicationDbContext samo kada se koristi umijesto za cijelo vrijme lifetime-a servisa
                using (var scope = _scopeFactory.CreateScope())
                    Resolver.UpdateRanks(scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(), _logger);
                _logger.LogInformation("Rank update finished.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update ranks failed!");
            }

            SetTimer();
        }

        private void SetTimer()
        {
            var timeDiff = _updateTime - DateTime.UtcNow.TimeOfDay;
            if (timeDiff < TimeSpan.Zero)
                timeDiff += TimeSpan.FromHours(24);
            _timer.Change(timeDiff, Timeout.InfiniteTimeSpan);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            SetTimer();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            return Task.CompletedTask;
        }
    }
}
