using AppConfigsUtils.Impl;
using FluentAssertions;
using System;
using Xunit;
namespace AppConfigsUtils.UnitTests;

public class MemoryFileManagerTests
{
    [Fact]
    public void TryCreateDirectory()
    {
        // Arrange
        var fs = new MemoryFileManager();

        // Act
        fs.CreateDirectory("/<root>/test/again");

        // Assert
        fs.DeleteDirectory("/<root>/test/again").Should().BeTrue();
    }

    [Fact]
    public void TryCreateFile()
    {
        // Arrange
        var fs = new MemoryFileManager();

        // Act
        {
            using var file = fs.CreateFile("test.txt");
            var fileWriter = new StreamWriter(file!.Stream);
            fileWriter.Write("Hello, World!");

            fileWriter.Flush();
        }

        // Assert
        {
            using var file = fs.Open("test.txt", FileMode.Open);
            file.Should().NotBeNull();
            var fileReader = new StreamReader(file!.Stream);
            fileReader.ReadToEnd().Should().Be("Hello, World!");
        }
    }

    [Fact]
    public void TryDeleteFile()
    {
        // Arrange
        var fs = new MemoryFileManager();
        fs.CreateFile("lol.txt");
        fs.CreateFile("lmao");
        fs.CreateFile("lulz.png");

        // Act
        fs.Delete("lmao");

        // Assert
        fs.Open("lmao", FileMode.Open).Should().BeNull();
    }

    [Fact]
    public void TryCreateFileInsideDirectory()
    {
        // Arrange
        var fs = new MemoryFileManager();
        fs.CreateDirectory("base/dir");

        // Act
        fs.CreateFile("base/dir/maybe");
        fs.CreateFile("base/dir/okay.txt");

        // Assert
        fs.Exists("base/dir/maybe").Should().BeTrue();
        fs.Exists("base/dir/okay.txt").Should().BeTrue();
    }

    [Fact]
    public void TryDeleteDirectory()
    {
        // Arrange
        var fs = new MemoryFileManager();
        fs.CreateDirectory("base/dir");
        fs.CreateFile("base/dir/maybe");
        fs.CreateFile("base/dir/okay.txt");

        // Act
        fs.DeleteDirectory("base/dir");

        // Assert
        fs.DirectoryExists("base/dir").Should().BeFalse();
        fs.Exists("base/dir/maybe").Should().BeFalse();
    }

    [Fact]
    public void TryCreateFileWithoutDirectory()
    {
        // Arrange
        var fs = new MemoryFileManager();
        
        // Act
        fs.CreateFile("base/dir/maybe");

        // Assert
        fs.Open("base/dir/maybe", FileMode.Open).Should().BeNull();
    }
}