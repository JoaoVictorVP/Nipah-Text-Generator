using FluentAssertions;
using LiteDB;
using System;
using Xunit;

namespace LiteDBForkJoin.UnitTests;

public class DBUtilsTest
{
    static (LiteDatabase dbFrom, LiteDatabase dbTo) Arrange()
    {
        var dbFrom = new LiteDatabase(new MemoryStream(32));
        var dbTo = new LiteDatabase(new MemoryStream(32));
        dbFrom.GetCollection("Loc").InsertBulk(new[] { new BsonDocument()
        {
            { "Name", "John" },
            { "Age", 100 }
        },
        new BsonDocument()
        {
            { "_id", "Vessel" },

            { "Cause", "Consequence" },
            { "Maybe", "You're right" },
            { "Or", "Maybe not" },

            { "Arr", new BsonArray(new BsonValue[] { 100, 200, 300 }) }
        } });
        dbFrom.GetCollection("Lmao").Insert(new BsonDocument()
        {
            { "Cuz", "lulz" }
        });

        dbTo.GetCollection("Loc").Insert(new BsonDocument()
        {
            { "_id", "Vessel" },
            { "Consequence", "Is life" },
            { "Or", "Not" },

            { "Arr", new BsonArray(new BsonValue[] { 300, 500 }) }
        });

        return (dbFrom, dbTo);
    }

    [Fact]
    public void LiteDBJoinREP_JAR()
    {
        // Arrange
        var strategy = MergeStrategy.ReplaceEveryProperty | MergeStrategy.JustAddArray;
        var (dbFrom, dbTo) = Arrange();

        // Act
        DBUtils.Join(dbFrom, dbTo, strategy);

        // Assert
        var loc = dbTo.GetCollection("Loc");
        loc.FindById("Vessel")["Or"].AsString.Should().Be("Maybe not");
        loc.FindOne(x => x["Name"].IsString)["Age"].AsInt32.Should().Be(100);
        dbTo.CollectionExists("Lmao").Should().BeTrue();
        var lmao = dbTo.GetCollection("Lmao");
        lmao.FindOne(x => x["Cuz"].IsString)["Cuz"].Should().Be("lulz");

        var locArr = loc.FindById("Vessel")["Arr"].AsArray;
        locArr.Count.Should().Be(5);
        locArr.SequenceEqual(new BsonValue[] { 300, 500, 100, 200, 300 }).Should().BeTrue();
    }

    [Fact]
    public void LiteDBJoinREP_ROAAKE()
    {
        // Arrange
        var strategy = MergeStrategy.ReplaceEveryProperty | MergeStrategy.ReplaceOrAddAndKeepExisting;
        var (dbFrom, dbTo) = Arrange();

        // Act
        DBUtils.Join(dbFrom, dbTo, strategy);

        // Assert
        var loc = dbTo.GetCollection("Loc");
        loc.FindById("Vessel")["Or"].AsString.Should().Be("Maybe not");
        loc.FindOne(x => x["Name"].IsString)["Age"].AsInt32.Should().Be(100);
        dbTo.CollectionExists("Lmao").Should().BeTrue();
        var lmao = dbTo.GetCollection("Lmao");
        lmao.FindOne(x => x["Cuz"].IsString)["Cuz"].Should().Be("lulz");

        var locArr = loc.FindById("Vessel")["Arr"].AsArray;
        locArr.Count.Should().Be(3);
        locArr.SequenceEqual(new BsonValue[] { 100, 200, 300 }).Should().BeTrue();
    }

    [Fact]
    public void LiteDBJoinREP_REA()
    {
        // Arrange
        var strategy = MergeStrategy.ReplaceEveryProperty | MergeStrategy.ReplaceEntireArray;
        var (dbFrom, dbTo) = Arrange();

        // Act
        DBUtils.Join(dbFrom, dbTo, strategy);

        // Assert
        var loc = dbTo.GetCollection("Loc");
        loc.FindById("Vessel")["Or"].AsString.Should().Be("Maybe not");
        loc.FindOne(x => x["Name"].IsString)["Age"].AsInt32.Should().Be(100);
        dbTo.CollectionExists("Lmao").Should().BeTrue();
        var lmao = dbTo.GetCollection("Lmao");
        lmao.FindOne(x => x["Cuz"].IsString)["Cuz"].Should().Be("lulz");

        var locArr = loc.FindById("Vessel")["Arr"].AsArray;
        locArr.Count.Should().Be(3);
        locArr.SequenceEqual(new BsonValue[] { 100, 200, 300 }).Should().BeTrue();
    }

    [Fact]
    public void LiteDBJoinKOP_JAR()
    {
        // Arrange
        var strategy = MergeStrategy.KeepOldProperties | MergeStrategy.JustAddArray;
        var (dbFrom, dbTo) = Arrange();

        // Act
        DBUtils.Join(dbFrom, dbTo, strategy);

        // Assert
        var loc = dbTo.GetCollection("Loc");
        loc.FindById("Vessel")["Or"].AsString.Should().Be("Not");
        loc.FindOne(x => x["Name"].IsString)["Age"].AsInt32.Should().Be(100);
        dbTo.CollectionExists("Lmao").Should().BeTrue();
        var lmao = dbTo.GetCollection("Lmao");
        lmao.FindOne(x => x["Cuz"].IsString)["Cuz"].Should().Be("lulz");

        var locArr = loc.FindById("Vessel")["Arr"].AsArray;
        locArr.Count.Should().Be(2);
        locArr.SequenceEqual(new BsonValue[] { 300, 500 }).Should().BeTrue();
    }

    [Fact]
    public void LiteDBJoinRED_JAR()
    {
        // Arrange
        var strategy = MergeStrategy.ReplaceEntireDocument | MergeStrategy.JustAddArray;
        var (dbFrom, dbTo) = Arrange();

        // Act
        DBUtils.Join(dbFrom, dbTo, strategy);

        // Assert
        var loc = dbTo.GetCollection("Loc");
        loc.FindById("Vessel")["Or"].AsString.Should().Be("Maybe not");
        loc.FindOne(x => x["Name"].IsString)["Age"].AsInt32.Should().Be(100);
        dbTo.CollectionExists("Lmao").Should().BeTrue();
        var lmao = dbTo.GetCollection("Lmao");
        lmao.FindOne(x => x["Cuz"].IsString)["Cuz"].Should().Be("lulz");

        var locArr = loc.FindById("Vessel")["Arr"].AsArray;
        locArr.Count.Should().Be(3);
        locArr.SequenceEqual(new BsonValue[] { 100, 200, 300 }).Should().BeTrue();
    }
}