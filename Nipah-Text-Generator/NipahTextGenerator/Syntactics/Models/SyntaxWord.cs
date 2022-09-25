namespace NipahTextGenerator.Syntactics.Models;

public class SyntaxWord
{
    public string Word { get; set; } = "";
    public List<SyntaxRelations> Relations { get; set; } = new(32);
}
