using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Microsoft.Extensions.Logging;
using MessagePack;

namespace Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ThreadPool.SetMaxThreads(1000, 1000);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder defaultBuilder =
                Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    IWebHostBuilder bld = webBuilder.UseKestrel(
                        options =>
                        {   // WORKAROUND: Accept HTTP/2 only to allow insecure HTTP/2 connections during development.
                            options.ConfigureEndpointDefaults(
                                endpointOptions =>
                                {
                                    endpointOptions.Protocols = HttpProtocols.Http2;
                                }
                            );
                            //options.Listen(System.Net.IPAddress.Parse("xxx.xxx.xxx.xxx"), 12345);
                        });
                    bld.UseStartup<Startup>();
                });

            return defaultBuilder;
        }
    }
}
