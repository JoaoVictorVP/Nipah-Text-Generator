using AppConfigsUtils.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigsUtils.Impl;

public class MemoryFileManager : IFileManager
{
    readonly FSDirectory root = new("<root>");

    public bool Exists(string file) => root.TryNavigate(file) is not null;

    public Stream? Open(string file, FileMode mode)
    {
        var fileNode = root.TryNavigate(file) as FSFile;
        return fileNode?.Contents;
    }

    public void Delete(string file)
    {
        var handle = root.TryNavigate(file);
        handle?.Parent?.Delete(handle.Name);
    }

    public ValueTask DeleteAsync(string file, CancellationToken ct = default)
    {
        Delete(file);
        return ValueTask.CompletedTask;
    }

    abstract record FSNode(string Name) : IDisposable
    {
        public FSDirectory? Parent { get; init; }
        string? cPath;
        public string Path => cPath ??= CalcPath();
        string CalcPath()
            => Parent is not null
            ? string.Join('/', Parent.CalcPath(), Name)
            : Name;

        public abstract void Dispose();
    }

    record FSDirectory(string Name) : FSNode(Name)
    {
        readonly Dictionary<string, FSNode> files = new(32);

        public void Add(FSNode node)
            => files[node.Name] = node with { Parent = this };
        public void Delete(string name)
        {
            files.Remove(name, out FSNode? node);
            node?.Dispose();
        }

        static readonly char[] pathSeparators = new[] { '/', '\\' };
        public FSNode? TryNavigate(string path)
        {
            string[] parts = path.Split(pathSeparators);
            return DoTryNavigate(parts);
        }
        FSNode? DoTryNavigate(ReadOnlySpan<string> parts)
            => files.ContainsKey(parts[0])
            ? parts.Length is 1
                ? files[parts[0]]
                : files[parts[0]] is FSDirectory fsDir
                    ? fsDir.DoTryNavigate(parts[1..])
                    : null
            : null;

        public override void Dispose()
        {
            foreach (var file in files)
                file.Value.Dispose();
        }
    }
    record FSFile(string Name, Stream Contents) : FSNode(Name)
    {
        public override void Dispose() => Contents?.Dispose();
    }
}
