using NipahTokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextGenerator.Syntactics;

public class SyntaxTrainer
{
    readonly Tokenizer tokenizer;
    readonly TokenizerOptions tokenizerOptions;

    public SyntaxTrainer()
    {
        tokenizer = new();
        tokenizerOptions = new(
            TokenizerOptions.DefaultSeparators,
            Array.Empty<Scope>(),
            TokenizerOptions.DefaultEndOfLines,
            TokenizerOptions.DefaultAggregators
        );
    }
    public void Train(string text)
    {
        var tokens = tokenizer.Tokenize(text, tokenizerOptions);

        for (int i = 0; i < tokens.Count; i++)
        {
            Token token = tokens[i];
            if (token.Type is TokenType.EOF) continue;

            Token? before = i is 0 ? (Nullable<Token>)null : tokens[i - 1];
            Token? next = i >= tokens.Count ? (Nullable<Token>)null : tokens[i + 1];

            var word = GetEntry(token.Text);



        }
    }
}
