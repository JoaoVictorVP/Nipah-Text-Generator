namespace NipahTextGenerator.Syntactics.Models;

public class SyntaxWord
{
    public readonly string Word;

    public SyntaxWord(string word)
    {
        Word = word;
    }

    public SyntaxRelations Relations { get; set; } = new();
}
