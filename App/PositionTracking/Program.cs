using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PositionTracking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
#if DEBUG
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                Data.DbInitializer.Initialize(services);

                //var mailSender = services.GetRequiredService<EmailSender>();

                //mailSender.SendAsync("sandro.vlastelica@gmail.com", "Test -PT","This is a test message").Wait();
            }
#endif
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
