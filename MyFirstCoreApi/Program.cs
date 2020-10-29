using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Unity.Microsoft.DependencyInjection;

namespace MyFirstCoreApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseUnityServiceProvider();   //µù¥UUnity
    }
}
