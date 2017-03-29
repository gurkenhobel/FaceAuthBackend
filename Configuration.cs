using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FaceAuthService
{
    class Configuration
    {

        public static IConfigurationRoot LoadConfig()
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile(Directory.GetCurrentDirectory() + "cfg/config.json");

            return builder.Build();
        }
    }
}
