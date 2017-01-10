using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Sample.DotNetFramework.MVC6
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.ThreadCount = Environment.ProcessorCount;
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("http://*:5000")
                .Build();

            host.Run();
        }
    }
}
