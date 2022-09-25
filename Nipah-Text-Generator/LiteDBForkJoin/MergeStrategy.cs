namespace LiteDBForkJoin;

[Flags]
public enum MergeStrategy
{
    /// <summary>
    /// Will keep the old (current) properties from destination
    /// </summary>
    KeepOldProperties = 1,
    /// <summary>
    /// Will replace all properties with new values, but keep the unchanged old ones
    /// </summary>
    ReplaceEveryProperty = 2,
    /// <summary>
    /// Will replace the entire document
    /// </summary>
    ReplaceEntireDocument = 4,
    /// <summary>
    /// Will just add new values to arrays, not caring about same-indexed ones
    /// </summary>
    JustAddArray = 8,
    /// <summary>
    /// Will replace if indexes matches (e.g. from_idx[0] and to_idx[0]) and add if indexes doesn't match, otherwise will keep everything same
    /// </summary>
    ReplaceOrAddAndKeepExisting = 16,
    /// <summary>
    /// Will replace entire arrays (without caring to documents too)
    /// </summary>
    ReplaceEntireArray = 32
}
public static class MergeStrategyExtensions
{
    public static bool IsKeepOldProperties(this MergeStrategy strategy) => strategy.HasFlag(MergeStrategy.KeepOldProperties);
    public static bool IsReplaceEveryProperty(this MergeStrategy strategy) => strategy.HasFlag(MergeStrategy.ReplaceEveryProperty);
    public static bool IsReplaceEntireDocument(this MergeStrategy strategy) => strategy.HasFlag(MergeStrategy.ReplaceEntireDocument);
    public static bool IsJustAddArray(this MergeStrategy strategy) => strategy.HasFlag(MergeStrategy.JustAddArray);
    public static bool IsReplaceOrAddAndKeepExisting(this MergeStrategy strategy) => strategy.HasFlag(MergeStrategy.ReplaceOrAddAndKeepExisting);
    public static bool IsReplaceEntireArray(this MergeStrategy strategy) => strategy.HasFlag(MergeStrategy.ReplaceEntireArray);
}