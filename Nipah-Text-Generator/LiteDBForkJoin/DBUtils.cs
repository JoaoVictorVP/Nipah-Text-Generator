using LiteDB;

namespace LiteDBForkJoin;

public static class DBUtils
{
    public static void Join(ILiteDatabase from, ILiteDatabase to, MergeStrategy strategy = MergeStrategy.ReplaceEveryProperty | MergeStrategy.JustAddArray)
    {
        var collectionNames = from.GetCollectionNames();
        foreach (var colName in collectionNames)
        {
            var colFrom = from.GetCollection(colName);
            var all = colFrom.FindAll();
            bool needJoinCol = to.CollectionExists(colName);
            var colTo = to.GetCollection(colName);
            if (needJoinCol is false)
            {
                colTo.InsertBulk(all);
                continue;
            }
            foreach (var docFrom in all)
            {
                if (docFrom.TryGetValue("_id", out var docFromId))
                {
                    var docTo = colTo.FindById(docFrom["_id"]);
                    if (docTo is null)
                        colTo.Insert(docFrom);
                    else
                    {
                        if (strategy.IsReplaceEntireDocument())
                            colTo.Update(docFrom);
                        else
                        {
                            JoinDocument(docFrom, docTo, strategy);
                            colTo.Update(docFromId, docTo);
                        }
                    }
                }
                else
                    colTo.Insert(docFrom);
            }
        }
    }
    static void JoinDocument(BsonDocument docFrom, BsonDocument docTo, MergeStrategy strategy)
    {
        if (strategy.IsReplaceEveryProperty())
        {
            foreach (var prop in docFrom)
            {
                if (prop.Value.IsArray && docTo[prop.Key] is BsonArray toArray)
                {
                    if (strategy.IsReplaceEntireArray() is false)
                    {
                        JoinArray(prop.Value.AsArray, toArray, strategy);
                        continue;
                    }
                }
                docTo[prop.Key] = prop.Value;
            }
        }
        else if (strategy.IsKeepOldProperties())
        {
            foreach (var prop in docFrom)
            {
                if (docTo.ContainsKey(prop.Key) is false)
                {
                    if (prop.Value.IsArray && docTo[prop.Key] is BsonArray toArray)
                    {
                        if (strategy.IsReplaceEntireArray() is false)
                        {
                            JoinArray(prop.Value.AsArray, toArray, strategy);
                            continue;
                        }
                    }
                    docTo[prop.Key] = prop.Value;
                }
            }
        }
    }
    static void JoinArray(BsonArray arrFrom, BsonArray arrTo, MergeStrategy strategy)
    {
        int fromCount = arrFrom.Count;
        int toCount = arrTo.Count;
        for (int i = 0; i < fromCount; i++)
        {
            var fromVal = arrFrom[i];
            if (i < toCount && strategy.IsJustAddArray() is false)
            {
                var toVal = arrTo[i];
                if (fromVal.IsDocument && toVal.IsDocument)
                {
                    JoinDocument(fromVal.AsDocument, toVal.AsDocument, strategy);
                    continue;
                }
                else if (fromVal.IsArray && toVal.IsArray)
                {
                    JoinArray(fromVal.AsArray, toVal.AsArray, strategy);
                    continue;
                }
                else
                    arrTo[i] = fromVal;
            }
            else
                arrTo.Add(fromVal);
        }
    }
}
