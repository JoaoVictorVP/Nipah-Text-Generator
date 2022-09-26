using AppLocalizationUtils;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppLocalization.UnitTests;

public class LocalizationManagerTests
{
    [Fact]
    public void TryHooks()
    {
        // Arrange
        const string simpleLocEn = @"
{
    ""wyn"": ""Hello stranger, what is your name?""
}";
        const string simpleLocPt = @"
{
    ""wyn"": ""Olá estranho, qual o seu nome?""
}";
        var locMan = new LocalizationManager();
        var locEn = Localization.Load(simpleLocEn);
        var locPt = Localization.Load(simpleLocPt);
        locMan.Add("en-US", locEn);
        locMan.Add("pt-BR", locPt);

        string textToShow = "";

        // Act
        locMan.Hook("wyn", nv => textToShow = nv);

        // Assert
        textToShow.Should().Be("Hello stranger, what is your name?");
        locMan.SetCurrent("pt-BR");
        textToShow.Should().Be("Olá estranho, qual o seu nome?");
    }
}
