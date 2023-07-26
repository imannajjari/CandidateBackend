
using Microsoft.Extensions.Configuration;

namespace Candidate.Core.Widgets.Config;

public static  class ConfigWidget
{
    private static IConfigurationRoot _config;
    public static IConfigurationRoot GetMyConfig()
    {
        if (_config != null) return _config;
        _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        return _config;
    }

    public static T GetConfigValue<T>(string configName)
    {
        var myConfig = GetMyConfig();
        var result = myConfig.GetValue<T>(configName);
        return result;
    }

    public static IConfigurationSection GetConfigSection(string sectionName)
    {
        var myConfig = GetMyConfig();
        var result = myConfig.GetSection(sectionName);
        return result;
    }

}