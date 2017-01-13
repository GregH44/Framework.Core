using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Framework.Core.Configuration
{
    public static class ConfigurationManager
    {
        public static IConfigurationRoot Configuration { get; private set; }

        public static IConfigurationRoot Configure(string contentRootPath)
        {
            if (Configuration != null)
                return Configuration;

            var environmentVariables = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            var builder = new ConfigurationBuilder()
                .SetBasePath(contentRootPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables();

            var profile = environmentVariables.GetSection("COMPUTERNAME");

            if (File.Exists($"appsettings.{profile.Value}.json"))
            {
                builder.AddJsonFile($"appsettings.{profile.Value}.json", optional: false);
            }
            else
            {
                builder.AddJsonFile("appsettings.Production.json", optional: false);
            }

            Configuration = builder.Build();

            return Configuration;
        }

        public static string GetValue(string jsonPath)
        {
            return Configuration[jsonPath];
        }
    }
}
