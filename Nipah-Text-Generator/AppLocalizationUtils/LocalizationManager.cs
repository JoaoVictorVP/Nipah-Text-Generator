using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLocalizationUtils;

public class LocalizationManager
{
    readonly Dictionary<string, Localization> localizations = new(32);
    Localization? current;

    public event Action<Localization>? OnCurrentChanged;

    public LocalizationManager()
    {
        OnCurrentChanged += changedTo =>
        {
            ExecuteHooks(changedTo);
        };
    }

    public void Add(string language, Localization localization)
    {
        localizations[language] = localization;
        if (current is null)
            SetCurrent(language);
    }

    public void SetCurrent(string language)
    {
        var oldCurrent = current;
        current = Get(language) ?? current;
        if(oldCurrent != current && current is not null)
            OnCurrentChanged?.Invoke(current);
    }

    public Localization? Get(string language)
        => localizations.TryGetValue(language, out var loc)
        ? loc
        : null;

    readonly List<WeakReference<LocalizationHook>> hooks = new(32);
    public LocalizationHook Hook(string entry, Action<string> localizedTextCallback)
    {
        var result = current?.Get(entry);
        if (result is not null)
            localizedTextCallback(result);
        var hook = new LocalizationHook(entry, localizedTextCallback);
        hooks.Add(new(hook));
        return hook;
    }
    void ExecuteHooks(Localization localization)
    {
        for (int i = 0; i < hooks.Count; i++)
        {
            WeakReference<LocalizationHook>? maybeHook = hooks[i];
            if (maybeHook.TryGetTarget(out var hook))
                hook.LocalizedTextCallback(localization.Get(hook.Hooking));
            else
                hooks.RemoveAt(i);
        }
    }

    public record class LocalizationHook(string Hooking, Action<string> LocalizedTextCallback);
}
