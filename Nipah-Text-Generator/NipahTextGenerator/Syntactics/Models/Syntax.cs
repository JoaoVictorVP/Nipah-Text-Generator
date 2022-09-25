using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextGenerator.Syntactics.Models;

public class Syntax
{
    public string Language { get; set; }
    List<SyntaxWord> words = new(32);
    Dictionary<string, SyntaxWord> indexedWords = new(32);

    SyntaxWord GetEntry(string word)
    {
        if(indexedWords.TryGetValue(word, out SyntaxWord result) is false)
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

    public void Train()
    {

    }
}
