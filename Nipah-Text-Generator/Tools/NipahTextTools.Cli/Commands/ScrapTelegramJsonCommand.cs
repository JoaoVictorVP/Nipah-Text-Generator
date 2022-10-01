using NipahTextTools.Cli.Settings;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace NipahTextTools.Cli.Commands;

public class ScrapTelegramJsonCommand : Command<ScrapTelegramJsonSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] ScrapTelegramJsonSettings settings)
    {
        try
        {
            StringBuilder sb = new StringBuilder(32);
            AnsiConsole.MarkupLine($"Scraping telegram json conversation [bold yellow]'{settings.File}'[/] to [bold yellow]'{settings.Output}'[/]");

            var root = JsonNode.Parse(File.ReadAllText(settings.File))!.AsObject();

            var messages = root["messages"]?.AsArray();
            if (messages is null) throw new Exception("Telegram JSON doesn't have messages");
            foreach(var messageNode in messages)
            {
                var message = messageNode?.AsObject();
                if(message is not null)
                {
                    var from = message["from"] as JsonValue;
                    if (from is not null && message["text"] is not null and JsonValue text)
                    {
                        string content = text.GetValue<string>();
                        if(content is not (null or ""))
                            sb.AppendLine($"{from.GetValue<string>()}: {content}");
                    }
                }
            }
            File.WriteAllText(settings.Output, sb.ToString());
            AnsiConsole.MarkupLine("Done!");
            return 0;
        }
        catch(Exception ex)
        {
            AnsiConsole.WriteException(ex);
            return 1;
        }
    }
}
