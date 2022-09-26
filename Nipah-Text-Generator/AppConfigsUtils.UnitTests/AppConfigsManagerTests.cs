using AppConfigsUtils.Impl;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppConfigsUtils.UnitTests;

public class AppConfigsManagerTests
{
    static (Func<JsonConfigsManager> factory, MemoryFileManager fs) Prepare()
    {
        return (new(() => new()), new());
    }

    [Fact]
    public async ValueTask TryGetSection()
    {
        // Arrange
        var (fac, fs) = Prepare();
        var configsManager = new AppConfigsManager("Configs", "Test", fac, fs);

        // Act
        var section = await configsManager.GetSection("Localization");

        // Assert
        section.Should().NotBeNull();
    }

    [Fact]
    public async Task TryGetSetAndSaveSection()
    {
        // Arrange
        var (fac, fs) = Prepare();
        var configsManager = new AppConfigsManager("Configs", "Test", fac, fs);

        // Act
        var section = await configsManager.GetSection("Localization");
        await section!.Set("language", "pt-br");
        await section!.Save();

        // Assert
        var sectionAgain = await configsManager.GetSection("Localization");
        sectionAgain.Should().NotBeNull();
        (await section.GetOrDefault<string>("language", null)).Should().Be("pt-br");
        (await section.GetOrDefault("lol", 300)).Should().Be(300);
    }

    [Fact]
    public async Task TryRemoveFromSection()
    {
        // Arrange
        var (fac, fs) = Prepare();
        var configsManager = new AppConfigsManager("Configs", "Test", fac, fs);
        var section = await configsManager.GetSection("Localization");
        await section!.Set("additional_prop", 300);

        // Act
        bool removeStatus = await section!.Remove("additional_prop");

        // Assert
        removeStatus.Should().BeTrue();
        (await section!.GetOrDefault("additional_prop", (int?)null)).Should().Be(null);
    }
}
