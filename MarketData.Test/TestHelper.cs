using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MarketData.Test
{
    public class TestHelper
    {
        public static IConfigurationRoot ReadFromAppSettings()
        {
            string workingDirectory = Environment.CurrentDirectory;
            // or: Directory.GetCurrentDirectory() gives the same result


            // This will get the current PROJECT directory
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            return new ConfigurationBuilder()
                .SetBasePath(projectDirectory)
                .AddJsonFile(@"appsettings.json", false)
                .Build();
        }
    }
}
