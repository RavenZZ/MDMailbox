using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Mailbox.Configuration
{
    public class Configuration
    {
        private Configuration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            ConfigurationBinder.Bind(configuration, this);            
        }
        #region 单例

        public static Configuration Instance
        {
            get { return Nested.instance; }
        }


        private class Nested
        {
            static Nested()
            {
            }
            internal static readonly Configuration instance = new Configuration();
        } 
        #endregion

        public string test { get; set; }

        public MongoConfig MongoDB { get; set; }


    }

    public class MongoConfig
    {
        public string Uri { get; set; }
    }



}
