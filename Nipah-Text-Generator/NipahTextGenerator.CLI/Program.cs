
using Spectre.Console;

var console = AnsiConsole.Create(new()
{
    ColorSystem = ColorSystemSupport.Detect,
    Ansi = AnsiSupport.Detect,
    Enrichment = new() { UseDefaultEnrichers = true },
    Interactive = InteractionSupport.Detect
});

var menu = new SelectionPrompt<string>();