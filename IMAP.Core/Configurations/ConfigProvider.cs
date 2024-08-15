using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace IMAP.Core;
public class ConfigProvider
{
    private static IConfiguration _configuration;
    public ConfigProvider()
    {
        _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
    }

    public T GetConfigValueByKey<T>(string key)
    {
        Type typeOfCfg = typeof(T);
        var instance = Activator.CreateInstance(typeOfCfg);

        foreach (var property in typeOfCfg.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.CanWrite)
            {
                var configKey = $"{key}:{property.Name}";
                var configValue = _configuration[configKey];

                if (configValue != null)
                    property.SetValue(instance, configValue);
            }
        }

        return (T)instance;
    }
}
