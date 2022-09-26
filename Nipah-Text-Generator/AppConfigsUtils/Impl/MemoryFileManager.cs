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

    public string Root => "<root>/";

    public bool Exists(string file) => root.TryNavigate(file) is FSFile and not null;

    public bool DirectoryExists(string directory) => root.TryNavigate(directory) is FSDirectory and not null;

    public FileHandle? Open(string file, FileMode mode)
    {
        if(mode is FileMode.Create)
        {
            Delete(file);
            return CreateFile(file);
        }
        else if(mode is FileMode.CreateNew)
            return Exists(file) ? null : CreateFile(file);

        return root.TryNavigate(file) is not FSFile fileNode
            ? mode is FileMode.OpenOrCreate
                ? CreateFile(file)
                : null
            : new(fileNode.Contents, false);
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

    public bool DeleteDirectory(string path)
    {
        var dir = root.TryNavigate(path);
        if (dir is null)
            return false;
        dir.Parent?.Delete(dir.Name);
        return true;
    }

    public FileHandle? CreateFile(string path)
    {
        string? bdirP = Path.GetDirectoryName(path);
        if (bdirP is null) return null;
        var bdir = root.TryNavigate(bdirP);
        if (bdir is not null and FSDirectory bdirFact)
        {
            var stream = new MemoryStream(320);
            bdirFact.Add(new FSFile(Path.GetFileName(path), stream));
            return new(stream, false);
        }
        else
            return null;
    }

    public bool CreateDirectory(string path)
    {
        string[] parts = path.Split(FSDirectory.PathSeparators, StringSplitOptions.RemoveEmptyEntries);
        DoCreateDirectory(parts, root);
        return true;
    }
    FSDirectory? DoCreateDirectory(ReadOnlySpan<string> parts, FSDirectory dir)
        => parts switch
        {
            { IsEmpty: false } => DoCreateDirectory(parts[1..], 
                dir.TryGet(parts[0]) as FSDirectory ?? dir.Add(new FSDirectory(parts[0]))),
            _ => dir
        };

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

        public TFSNode Add<TFSNode>(TFSNode node) where TFSNode : FSNode
            => (TFSNode)(files[node.Name] = node with { Parent = this });
        public FSNode? TryGet(string name)
        {
            return files.TryGetValue(name, out var node)
                ? node
                : null;
        }
        public void Delete(string name)
        {
            files.Remove(name, out FSNode? node);
            node?.Dispose();
        }

        public static readonly char[] PathSeparators = new[] { '/', '\\' };
        public FSNode? TryNavigate(string path)
        {
            if (path is "") return this;
            string[] parts = path.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length is 0) return this;
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
