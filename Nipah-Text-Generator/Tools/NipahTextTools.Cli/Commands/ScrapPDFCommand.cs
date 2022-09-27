using NipahTextTools.Cli.Settings;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace NipahTextTools.Cli.Commands;

public class ScrapPDFCommand : Command<ScrapPDFSettings>
{
    StringBuilder documentSB = new(3200);
    public override int Execute([NotNull] CommandContext context, [NotNull] ScrapPDFSettings settings)
    {
        documentSB.Clear();
        AnsiConsole.MarkupLine($"Scrapping pdf [bold yellow]'{settings.File}'[/] to [bold yellow]'{settings.Output}'[/]");
        using var doc = PdfDocument.Open(settings.File);
        foreach(var page in doc.GetPages())
        {
            AnsiConsole.MarkupLine($"Scrapping page [bold]{page.Number}[/]");
            string text = Process(page.Text);
            documentSB.AppendLine(text);
        }
        File.WriteAllText(documentSB.ToString(), settings.Output);
        AnsiConsole.MarkupLine("Done!");
        return 0;
    }

    StringBuilder processSB = new(320);
    string Process(string text)
    {
        processSB.Clear();
        bool canPlaceNewLine = false;
        foreach(var c in text)
        {
            if (c is '.' or ';' or ':' or '?' or '!' or ')')
                canPlaceNewLine = true;

            if (c is '\n' && canPlaceNewLine is false)
                continue;

            if (c is '\r')
                continue;

            processSB.Append(c);
        }

        return processSB.ToString();
    }
}
