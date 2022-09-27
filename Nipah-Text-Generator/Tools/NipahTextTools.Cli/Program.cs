
using NipahTextTools.Cli.Commands;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddBranch("scrap", scrap =>
    {
        scrap.AddCommand<ScrapPDFCommand>("pdf");
    });
});

return app.Run(args);