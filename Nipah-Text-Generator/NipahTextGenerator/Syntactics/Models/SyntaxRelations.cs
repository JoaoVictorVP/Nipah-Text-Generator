namespace NipahTextGenerator.Syntactics.Models;

public class SyntaxRelations
{
    public SyntaxRelation? Left { get; set; }
    public SyntaxRelation? Right { get; set; }
}

public record SyntaxRelation(SyntaxWord Word, int Times);
