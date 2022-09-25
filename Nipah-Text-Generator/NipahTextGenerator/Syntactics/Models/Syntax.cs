using NipahTokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextGenerator.Syntactics.Models;

public class Syntax
{
    public string Language { get; set; }
    readonly Tokenizer tokenizer;
    readonly TokenizerOptions tokenizerOptions;
    readonly List<SyntaxWord> words = new(32);
    readonly Dictionary<string, SyntaxWord> indexedWords = new(32);

    SyntaxWord GetEntry(string word)
    {
        if(indexedWords.TryGetValue(word, out SyntaxWord? result) is false || result is null)
        {
            result = new SyntaxWord(word);
            words.Add(result);
            indexedWords[word] = result;
        }
        return result;
    }

    public Syntax(string language)
    {
        Language = language;

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
