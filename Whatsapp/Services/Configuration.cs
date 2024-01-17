using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Whatsapp.Services
{
    static public class Configuration
    {

        public static string GetValue(string Key, string Value)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(@"C:\Users\user\Desktop\Whatsapp\Whatsapp\appsettings.json");
            return builder.Build().GetSection(Key)[Value];
        }

    }
}
