using AppConfigsUtils.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigsUtils.Impl;

public class JsonConfigsManager : IConfigsManager
{
    static readonly JsonSerializerSettings jsonSettings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
        NullValueHandling = NullValueHandling.Ignore
    };

    Action<Stream>? onSave;
    AppConfigs? configs;

    public JsonConfigsManager()
    {
        onSet += async prop =>
        {
            await ExecuteHooks();
        };
    }

    public ValueTask Load(Stream from, Action<Stream> onSave)
    {
        this.onSave = onSave;

        var reader = new StreamReader(from);
        string json = reader.ReadToEnd();
        configs = JsonConvert.DeserializeObject<AppConfigs>(json, jsonSettings) ?? new(32);

        return ValueTask.CompletedTask;
    }
    AppConfigs EnsureLoaded()
    {
        if (configs is null)
            throw new Exception("Load not called for this settings manager");
        return configs;
    }
    Action<Stream> EnsureOnSave()
    {
        return onSave is not null
            ? onSave
            : throw new Exception("Load not called for this settings manager");
    }

    event Func<string, ValueTask> onSet;
    public async ValueTask Set<T>(string property, T value)
    {
        ArgumentNullException.ThrowIfNull(value);
        var configs = EnsureLoaded();
        configs[property] = value;
        await (onSet?.Invoke(property) ?? ValueTask.CompletedTask);
    }

    public ValueTask<T?> GetOrDefault<T>(string property, T? defaultValue = default)
    {
        var configs = EnsureLoaded();
        return configs.TryGetValue(property, out var value) 
            ? ValueTask.FromResult((T?)value) 
            : ValueTask.FromResult(defaultValue);
    }

    public ValueTask<bool> Remove(string property)
    {
        var configs = EnsureLoaded();
        return ValueTask.FromResult(configs.Remove(property));
    }

    public ValueTask<bool> Contains(string property)
    {
        var configs = EnsureLoaded();
        return ValueTask.FromResult(configs.ContainsKey(property));
    }

    public ValueTask Clear()
    {
        var configs = EnsureLoaded();
        configs.Clear();
        return ValueTask.CompletedTask;
    }

    public ValueTask Save()
    {
        var configs = EnsureLoaded();
        var json = JsonConvert.SerializeObject(configs, jsonSettings);
        var onSave = EnsureOnSave();

        using var stream = new MemoryStream(320);
        using var writer = new StreamWriter(stream);
        writer.Write(json);
        writer.Flush();
        onSave(stream);

        return ValueTask.CompletedTask;
    }

    readonly List<WeakReference<ConfigsHook>> hooks = new(32);
    public async ValueTask<ConfigsHook> Hook<T>(string property, Action<T?> propertyCallback)
    {
        if (await Contains(property))
            propertyCallback(await GetOrDefault<T>(property));
        var hook = new ConfigsHook(property, val => propertyCallback((T)val));
        hooks.Add(new(hook));
        return hook;
    }
    async ValueTask ExecuteHooks()
    {
        for (int i = 0; i < hooks.Count; i++)
        {
            var maybeHook = hooks[i];
            if (maybeHook.TryGetTarget(out var hook))
                hook.PropertyCallback(await GetOrDefault<object>(hook.Property));
            else
                hooks.RemoveAt(i);
        }
    }
}
