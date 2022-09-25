using NipahTokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextGenerator.Syntactics.Models;

public class Syntax
{
    public string Language { get; set; }
    readonly List<SyntaxWord> words = new(32);
    readonly Dictionary<string, SyntaxWord> indexedWords = new(32);

    public SyntaxWord GetEntry(string word)
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
    }

    public override string ToString()
    {
        var sb = new StringBuilder(3200);
        foreach (var word in words)
            sb.AppendLine(BuildFor(word));
        return sb.ToString();
    }
    string BuildFor(SyntaxWord word)
        => $"{word.Word}:"
        + "\n Left" + BuildForRelations(CollectionsMarshal.AsSpan(word.Relations.Left), "")
        + "\n Right" + BuildForRelations(CollectionsMarshal.AsSpan(word.Relations.Right), "");
    string BuildForRelations(ReadOnlySpan<SyntaxRelation> relations, string str)
        => relations switch
        {
            { Length: > 0 } => $"\n   - {relations[0].Word.Word} ({relations[0].Times}x Times)"
                + BuildForRelations(relations[1..], str),
            _ => str
        };
}
