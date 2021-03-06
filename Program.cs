using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileSharingApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FileSharingApp
{
    public class Program
    {
        //Entry Point 
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Run Migration >> Update-Database
            using (var scope = host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;

                var dbContext = provider.GetRequiredService<ApplicationDbContext>();

                dbContext.Database.Migrate();

                //Seed.

            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //Web Server (Kestrel)
                    //read AppSettings.json
                    //Startup.cs
                    // WWWroot for Static Files >> Css , JS, Images , Fonts

                    webBuilder.UseStartup<Startup>();
                });
    }
}
