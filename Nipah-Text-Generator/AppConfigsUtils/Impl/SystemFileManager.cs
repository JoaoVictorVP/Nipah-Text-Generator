using AppConfigsUtils.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigsUtils.Impl;

public class SystemFileManager : IFileManager
{
    public string Root
    {
        get
        {
            if (OperatingSystem.IsWindows())
                return Path.GetPathRoot(Path.GetFullPath("./")) ?? "C:/";
            else if (OperatingSystem.IsLinux())
                return "/";
            return "/";
        }
    }

    public bool Exists(string file) => File.Exists(file);

    public bool DirectoryExists(string directory) => Directory.Exists(directory);

    public FileHandle? Open(string file, FileMode mode)
    {
        return File.Open(file, mode) is not null and Stream fileStream
            ? new(fileStream, true)
            : null;
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

    public FileHandle? CreateFile(string path)
    {
        try
        {
            if (Directory.Exists(Path.GetDirectoryName(path)) is false)
                return null;
            return new(File.Create(path), true);
        }
        catch
        {
            return null;
        }
    }

    public bool CreateDirectory(string path)
    {
        try
        {
            Directory.CreateDirectory(path);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool DeleteDirectory(string path)
    {
        if (Directory.Exists(path) is false)
            return false;
        Directory.Delete(path, true);
        return true;
    }
}
