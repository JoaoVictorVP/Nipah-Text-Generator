using NipahTokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextGenerator.Experimental.CausalityNetwork;

public static class Common
{
    public static readonly TokenizerOptions TokenizerOptions = new(
        TokenizerOptions.DefaultSeparators,
        Array.Empty<Scope>(),
        Array.Empty<EndOfLine>(),
        Array.Empty<SplitAggregator>()
    );
    public static readonly Tokenizer Tokenizer = new();
}
