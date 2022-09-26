using AppConfigsUtils.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigsUtils;

public class AppConfigsManager
{
    public const string ConfigsSectionExtension = "ncf";

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
        if (FileManager.DirectoryExists(appDir) is false)
            FileManager.CreateDirectory(appDir);
        return cachedPath = appDir;
    }

    Dictionary<string, IConfigsManager> cachedSections = new(32);
    public async ValueTask<IConfigsManager?> GetSection(string name)
    {
        if (cachedSections.TryGetValue(name, out var cachedSection))
            return cachedSection;
        var appDir = EnsurePath();
        string sectionPath = Path.Combine(appDir, Path.ChangeExtension(name, ConfigsSectionExtension));
        using var section = FileManager.Open(sectionPath, FileMode.OpenOrCreate);
        if (section is null)
            return null;
        var configs = ConfigsManagerFactory();
        await configs.Load(section.Stream, toSave =>
        {
            var sectionAgain = FileManager.Open(sectionPath, FileMode.Open);
            if (sectionAgain is not null)
                toSave.CopyTo(sectionAgain.Stream);
        });
        return cachedSections[name] = configs;
    }
}
