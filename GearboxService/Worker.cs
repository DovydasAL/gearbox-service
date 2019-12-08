using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GearboxService.Models;
using GearboxService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GearboxService
{
    public class Worker : IHostedService, IDisposable
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        private readonly ITwitterService _twitterService;
        private readonly IShiftService _shiftService;
        private Timer _timer;

        public Worker(ILogger<Worker> logger, IConfiguration config, ITwitterService twitterService, IShiftService shiftService)
        {
            _logger = logger;
            _config = config;
            _twitterService = twitterService;
            _shiftService = shiftService;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(Begin, null, TimeSpan.Zero, 
                TimeSpan.FromMinutes(int.Parse(_config["ScrapeInterval"])));

            return Task.CompletedTask;
        }

        private void Begin(object state)
        {
            List<string> codes = _twitterService.GetCodes();
            _shiftService.SubmitCodes(codes);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}