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
            bool needJoinCol = to.CollectionExists(colName) is false;
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
                        colTo.Insert(docTo);
                    else
                        JoinDocument(docFrom, docTo);
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
}
