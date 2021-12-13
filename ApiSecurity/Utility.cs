using Microsoft.Extensions.Configuration;
using System;

namespace ApiSecurity
{
    public class Utility
    {
        public static T GetKey<T>(string Key)
        {

            Type temp = typeof(T);

            ConfigurationRoot configuration = (ConfigurationRoot)new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            return (T)Convert.ChangeType(configuration.GetSection(Key).Value, temp);

        }
    }
}
