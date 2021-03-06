﻿using Hangfire;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HangFire.NetCore.ClientDemo
{
    public class Program
    {
        private static IConfigurationRoot _configuration { get; set; }

        public static async Task Main(string[] args)
        {
            ConfigureApplication();

            await Console.Out.WriteLineAsync("HangFire Client has started. Sending test message...");

            BackgroundJob.Enqueue(() => Console.WriteLine($"Hello TestHost Sever {DateTime.Now}!"));

            await Console.Out.WriteLineAsync("HangFire Client finished its work. Press return to exit...");
            await Console.In.ReadLineAsync();
        }

        private static void ConfigureApplication()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();

            GlobalConfiguration
                .Configuration
                .UseColouredConsoleLogProvider()
                .UseSqlServerStorage(_configuration.GetConnectionString("HangFire"));
        }
    }
}
