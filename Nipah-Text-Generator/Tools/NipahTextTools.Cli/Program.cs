
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
    });
});

return app.Run(args);