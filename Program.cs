using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Zembil
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
                    webBuilder.UseKestrel().UseUrls("http://localhost:5522", "http://10.6.196.207:5522").UseIISIntegration().UseStartup<Startup>();
                });
    }
}
