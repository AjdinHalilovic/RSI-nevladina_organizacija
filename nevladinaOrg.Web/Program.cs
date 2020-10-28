using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using nevladinaOrg.Web.Helpers;

namespace nevladinaOrg.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    DatabaseInitializer.Seed(scope.ServiceProvider);
                }
                catch (System.Exception e)
                  {
                    System.Diagnostics.Debug.WriteLine(e.InnerExceptionMessage());
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
    }
}
