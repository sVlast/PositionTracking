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

namespace PositionTracking.WebApi
{
    public class RankUpdateService : IHostedService, IDisposable
    {
        //interval kada Timer provjerava da li je prošlo vrijeme od zadnjeg updateRanks
        private const int timerInterval = 5 * 60 * 1000;
        private static readonly TimeSpan timeSpread = TimeSpan.FromMinutes(10);
        private readonly ILogger<RankUpdateService> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly Timer _timer;
        private readonly TimeSpan _updateTime;

        public RankUpdateService(ILogger<RankUpdateService> logger, ApplicationDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = context;
            _updateTime = configuration.GetValue<TimeSpan>("Settings:RankUpdateTime");
            _timer = new Timer(OnTimerInterval);

        }

        private void OnTimerInterval(object state)
        {
            try
            {
                _logger.LogInformation("Rank update started.");
                Resolver.UpdateRanks(_dbContext, _logger);
                _logger.LogInformation("Rank update finished.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Update ranks failed!");
            }

            //update timer

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
            //22 trenutno - vrijeme okidanja 1 > 
            var timeDiff = _updateTime - DateTime.UtcNow.TimeOfDay;
            if (timeDiff < TimeSpan.Zero)
                timeDiff += TimeSpan.FromHours(24);
            _timer.Change(timeDiff, Timeout.InfiniteTimeSpan);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            return Task.CompletedTask;
        }
    }
}
