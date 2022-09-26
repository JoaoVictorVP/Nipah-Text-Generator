using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigsUtils;

public record ConfigsHook(string Property, Action<object?> PropertyCallback);