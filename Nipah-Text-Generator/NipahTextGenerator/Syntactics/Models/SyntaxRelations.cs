using System.Runtime.InteropServices;

namespace NipahTextGenerator.Syntactics.Models;

public class SyntaxRelations
{
    public List<SyntaxRelation> Left { get; set; } = new(32);
    public List<SyntaxRelation> Right { get; set; } = new(32);

    public void AddLeft(string word)
    {
        var left = CollectionsMarshal.AsSpan(Left);
        foreach (ref var relation in left)
        {
            if (relation.Word.Word == word)
                relation = relation with { Word = relation.Word, Times = relation.Times + 1 };
        }
    }
    public void AddRight(string word)
    {
        var right = CollectionsMarshal.AsSpan(Right);
        foreach (ref var relation in right)
        {
            if (relation.Word.Word == word)
                relation = relation with { Word = relation.Word, Times = relation.Times + 1 };
        }
    }
}

public record SyntaxRelation(SyntaxWord Word, int Times);
