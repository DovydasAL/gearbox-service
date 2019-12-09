using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GearboxService.Models;
using GearboxService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GearboxService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<ITwitterService, TwitterService>();
                    services.AddSingleton<IShiftService, ShiftService>();
                    services.AddSingleton<IEmailService, EmailService>();
                });
    }
}