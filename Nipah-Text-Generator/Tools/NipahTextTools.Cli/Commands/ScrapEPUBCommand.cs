using NipahTextTools.Cli.Settings;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VersOne.Epub;

namespace NipahTextTools.Cli.Commands;

public class ScrapEPUBCommand : Command<ScrapEPUBSettings>
{
    readonly StringBuilder outputSB = new(3200);
    public override int Execute([NotNull] CommandContext context, [NotNull] ScrapEPUBSettings settings)
    {
        try
        {
            outputSB.Clear();
            AnsiConsole.MarkupLine($"Scraping epub [bold yellow]'{settings.File}'[/] to [bold yellow]'{settings.Output}'[/]");
            var book = EpubReader.ReadBook(settings.File);
            int count = 0;
            foreach (var textContentFile in book.ReadingOrder)
            {
                if (textContentFile.ContentType is EpubContentType.CSS or EpubContentType.IMAGE_GIF or EpubContentType.OEB1_CSS or EpubContentType.IMAGE_JPEG or EpubContentType.IMAGE_PNG or EpubContentType.IMAGE_SVG)
                    continue;

                AnsiConsole.MarkupLine($"Reading content from order {count}...");
                count++;

                var text = Process(textContentFile.Content);

                outputSB.AppendLine(text);
            }
            File.WriteAllText(settings.Output, outputSB.ToString());
            AnsiConsole.MarkupLine("Done!");
            return 0;
        }
        catch(Exception ex)
        {
            AnsiConsole.WriteException(ex);
            return 1;
        }
    }

    static readonly Regex stripHtml = new(@"<.*?>", RegexOptions.Singleline | RegexOptions.Compiled);
    readonly StringBuilder processSB = new(32);
    string Process(string text)
    {
        processSB.Clear();

        text = stripHtml.Replace(text, "");

        bool canPlaceNewLine = false;
        foreach (var c in text)
        {
            if (c is '.' or ';' or ':' or '?' or '!' or ')' || char.IsUpper(c))
                canPlaceNewLine = true;

            if (c is '\n')
            {
                if (canPlaceNewLine is false)
                    continue;
                else
                    canPlaceNewLine = false;
            }

            if (c is '\r')
                continue;

            processSB.Append(c);
        }

        return processSB.ToString();
    }
}
