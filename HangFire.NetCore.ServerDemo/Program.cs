using Hangfire;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HangFire.NetCore.ServerDemo
{
    public class Program
    {
        private static IConfigurationRoot _configuration { get; set; }
        private static BackgroundJobServer _backgroundJobServer { get; set; }

        public static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(GracefulServerShutdown);

            ConfigureApplication();

            await Console.Out.WriteLineAsync("HangFire Processing Server has started. Press any key to exit...");
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

            _backgroundJobServer = new BackgroundJobServer();
        }

        private static void GracefulServerShutdown(object sender, EventArgs e)
        {
            _backgroundJobServer.SendStop();
            _backgroundJobServer.Dispose();
        }
    }
}
