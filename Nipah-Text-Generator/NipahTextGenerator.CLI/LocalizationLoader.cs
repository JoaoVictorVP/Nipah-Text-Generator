using AppLocalizationUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextGenerator.CLI;

public static class LocalizationLoader
{
    static readonly Dictionary<string, Localization> cachedLocs = new(32);

    public static Localization? Load(string language)
    {
        if (cachedLocs.TryGetValue(language, out var cachedLoc))
            return cachedLoc;
        string path = Path.Combine("localization", Path.ChangeExtension(language, "json"));
        if (File.Exists(path) is false)
            return null;
        string json = File.ReadAllText(path);
        return cachedLocs[language] = Localization.Load(json);
    }
}
