using Commons.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace CSharpHttpClientExample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(configuration =>
            {
                string applicationLevel = Environment.GetEnvironmentVariable("ApplicationLevel");
                char seperator = System.IO.Path.AltDirectorySeparatorChar;

                configuration.AddJsonFile($"Properties{seperator}{applicationLevel}{seperator}appsettings.json", false, true);

            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(options =>
                {
                    int portRest = Environment.GetEnvironmentVariable("PortRest").ToInteger();
                    options.ListenAnyIP(portRest, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1;
                    });
                });
                webBuilder.UseStartup<Startup>();
            });
    }
}
