
using NipahTextTools.Cli.Commands;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddBranch("scrap", scrap =>
    {
        scrap.AddCommand<ScrapPDFCommand>("pdf")
            .WithDescription("Will try scrap some pdf to plain text")
            .WithExample(new[] { "scrap", "pdf", "C:/dev/some.pdf", "--output", "C:/dev/scraps/some.txt" });

        scrap.AddCommand<ScrapEPUBCommand>("epub")
            .WithDescription("Will try to scrap some epub to plain text")
            .WithExample(new[] { "scrap", "epub", "C:/dev/some.epub", "--output", "C:/dev/scraps/some.txt" });
    });
});

return app.Run(args);