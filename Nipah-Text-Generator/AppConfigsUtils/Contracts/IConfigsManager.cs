using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigsUtils.Contracts;

public interface IConfigsManager
{
    ValueTask Set<T>(string property, T value);
    ValueTask<T?> GetOrDefault<T>(string property, T? defaultValue = default);
    ValueTask<bool> Remove(string property);

    ValueTask<bool> Contains(string property);

    ValueTask Clear();

    ValueTask Load(Stream from, Action<Stream> onSave);

    ValueTask Save();

    ValueTask<ConfigsHook> Hook<T>(string property, Action<T?> propertyCallback);
}
