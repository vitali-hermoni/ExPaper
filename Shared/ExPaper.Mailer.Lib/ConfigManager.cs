using Microsoft.Extensions.Configuration;

namespace ExPaper.Mailer.Lib;

public class ConfigManager : IConfigManager
{
    private readonly IConfigManager _configManager;


    public ConfigManager(IConfigManager configManager)
    {
        _configManager = configManager;
    }

    public string EmailHost => _configManager.EmailHost;

    public string EmailUsername => _configManager.EmailUsername;

    public string EmailPassword => _configManager.EmailPassword;
}
