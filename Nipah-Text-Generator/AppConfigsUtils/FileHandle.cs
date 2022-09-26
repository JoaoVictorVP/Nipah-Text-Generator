using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigsUtils;

public record FileHandle(Stream Stream, bool NeedDispose) : IDisposable
{
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if (NeedDispose)
            Stream.Dispose();
        else
            if(Stream.CanSeek) Stream.Position = 0;
    }
}
