using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ConnectUs.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //    var host = new WebHostBuilder()
            //    .UseKestrel()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseIISIntegration()
            //    .UseStartup()
            //    .Build();

            //    host.Run();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
