using AppConfigsUtils.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigsUtils.Impl;

public class SystemFileManager : IFileManager
{
    public bool Exists(string file) => File.Exists(file);

    public Stream Open(string file, FileMode mode)
    {
        return File.Open(file, mode);
    }

    public void Write(string file, Stream stream)
    {
        var handle = File.OpenWrite(file);
        handle.Position = 0;
        stream.CopyTo(handle);
    }

    public async ValueTask WriteAsync(string file, Stream stream, CancellationToken ct = default)
    {
        var handle = File.OpenWrite(file);
        handle.Position = 0;
        await stream.CopyToAsync(handle, ct);
    }

    public void Delete(string file)
    {
        File.Delete(file);
    }

    public ValueTask DeleteAsync(string file, CancellationToken ct = default)
    {
        File.Delete(file);
        return ValueTask.CompletedTask;
    }
}
