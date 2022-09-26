using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigsUtils.Contracts;

public interface IFileManager
{
    bool Exists(string file);
    Stream? Open(string file, FileMode mode);
    void Delete(string file);
    ValueTask DeleteAsync(string file, CancellationToken ct = default);
}
