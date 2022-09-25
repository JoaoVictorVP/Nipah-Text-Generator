using System.Runtime.InteropServices;

namespace NipahTextGenerator.Syntactics.Models;

public class SyntaxRelations
{
    public List<SyntaxRelation> Left { get; set; } = new(32);
    public List<SyntaxRelation> Right { get; set; } = new(32);

    public void AddLeft(string word, Func<string, SyntaxWord> getEntry)
    {
        bool foundAny = false;
        var collection = CollectionsMarshal.AsSpan(Left);
        foreach (ref var relation in collection)
        {
            if (relation.Word.Word == word)
            {
                relation = relation with { Word = relation.Word, Times = relation.Times + 1 };
                foundAny = true;
            }
        }
        if(foundAny is false)
        {
            var entry = getEntry(word);
            Left.Add(new(entry, 1));
        }
    }
    public void AddRight(string word, Func<string, SyntaxWord> getEntry)
    {
        bool foundAny = false;
        var collection = CollectionsMarshal.AsSpan(Right);
        foreach (ref var relation in collection)
        {
            if (relation.Word.Word == word)
            {
                relation = relation with { Word = relation.Word, Times = relation.Times + 1 };
                foundAny = true;
            }
        }
        if (foundAny is false)
        {
            var entry = getEntry(word);
            Right.Add(new(entry, 1));
        }
    }
}

public record SyntaxRelation(SyntaxWord Word, int Times);
