using AppLocalizationUtils;
using FluentAssertions;
using System;
using Xunit;
namespace AppLocalization.UnitTests;

public class LocalizationTests
{
    [Fact]
    public void TrySimpleLocalization()
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

        // Act
        var locEn = Localization.Load(simpleLocEn);
        var locPt = Localization.Load(simpleLocPt);

        // Assert
        locEn.Get("wyn").Should().Be("Hello stranger, what is your name?");
        locPt.Get("wyn").Should().Be("Olá estranho, qual o seu nome?");
    }
}