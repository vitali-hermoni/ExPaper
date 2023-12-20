using Microsoft.Extensions.Configuration;

namespace ExPaper.Mailer.Lib;

public interface IConfigManager
{
    string EmailHost { get; }

    string EmailUsername { get; }

    string EmailPassword { get; }
}
