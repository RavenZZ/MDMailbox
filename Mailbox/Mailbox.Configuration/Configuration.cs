using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Mailbox.Configuration
{
    public class Configuration
    {
        //public static IConfigurationRoot configuration;

        public string getConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appSettings.json");

            var Configuration = builder.Build();
            var a = Configuration.GetSection("appSettings");
            return a.ToString();
        }



    }
}
