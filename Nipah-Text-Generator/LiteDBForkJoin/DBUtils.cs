using LiteDB;

namespace LiteDBForkJoin;

public static class DBUtils
{
    public static void Join(ILiteDatabase from, ILiteDatabase to)
    {
        var collectionNames = from.GetCollectionNames();
        foreach(var colName in collectionNames)
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
                        JoinDocument(docFrom, docTo);
                        colTo.Update(docFromId, docTo);
                    }
                }
                else
                    colTo.Insert(docFrom);
            }
        }
    }
    static void JoinDocument(BsonDocument docFrom, BsonDocument docTo)
    {
        foreach (var prop in docFrom)
            docTo[prop.Key] = prop.Value;
    }
    static void JoinArray(BsonArray arrFrom, BsonArray arrTo)
    {
        int fromCount = arrFrom.Count;
        int toCount = arrTo.Count;
        for (int i = 0; i < fromCount; i++)
        {
            var fromVal = arrFrom[i];
            if(i < toCount)
            {
                var toVal = arrTo[i];
                if (fromVal.IsDocument && toVal.IsDocument)
                {
                    JoinDocument(fromVal.AsDocument, toVal.AsDocument);
                    continue;
                }
                else if (fromVal.IsArray && toVal.IsArray)
                {
                    JoinArray(fromVal.AsArray, toVal.AsArray);
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
