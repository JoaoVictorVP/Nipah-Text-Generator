using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppLocalizationUtils;

public class Localization
{
    readonly Dictionary<string, string> entries = new(32);

    Localization(Dictionary<string, string> entries) => this.entries = entries;

    public static Localization Load(string json)
    {
        var entries = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new(32);
        return new(entries);
    }

    public string Get(string key, string defaultValue = "")
        => entries.TryGetValue(key, out var value)
        ? value
        : defaultValue;
}
