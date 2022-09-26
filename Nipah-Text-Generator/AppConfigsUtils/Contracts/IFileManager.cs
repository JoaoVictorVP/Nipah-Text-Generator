using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigsUtils.Contracts;

public interface IFileManager
{
    bool Exists(string file);
    bool DirectoryExists(string directory);
    FileHandle? CreateFile(string path);
    bool CreateDirectory(string path);
    bool DeleteDirectory(string path);
    FileHandle? Open(string file, FileMode mode);
    void Delete(string file);
    ValueTask DeleteAsync(string file, CancellationToken ct = default);
}
