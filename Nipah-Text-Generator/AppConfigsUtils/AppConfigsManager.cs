using AppConfigsUtils.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigsUtils;

public class AppConfigsManager
{
    public string ConfigsDirectory { get; }
    public string AppName { get; }
    public Func<IConfigsManager> ConfigsManagerFactory { get; }
    public IFileManager FileManager { get; }

    public AppConfigsManager(string configsDirectory, string appName, Func<IConfigsManager> configsManagerFactory, IFileManager fileManager)
    {
        ConfigsDirectory = configsDirectory;
        AppName = appName;
        ConfigsManagerFactory = configsManagerFactory;
        FileManager = fileManager;
    }

    string? cachedPath;
    string EnsurePath()
    {
        if (cachedPath is not null)
            return cachedPath;
        string appDir = Path.Combine(ConfigsDirectory, AppName);
        if (Directory.Exists(appDir) is false)
            Directory.CreateDirectory(appDir);
        return cachedPath = appDir;
    }

    public async ValueTask<IConfigsManager?> GetSection(string name)
    {
        var appDir = EnsurePath();
        using var section = FileManager.Open(name, FileMode.OpenOrCreate);
        if (section is null)
            return null;
        var configs = ConfigsManagerFactory();
        await configs.Load(section.Stream, toSave =>
        {
            var sectionAgain = FileManager.Open(name, FileMode.Open);
            if (sectionAgain is not null)
                toSave.CopyTo(sectionAgain.Stream);
        });
        return configs;
    }
}
