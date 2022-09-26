
using AppConfigsUtils;
using AppConfigsUtils.Impl;
using Spectre.Console;

var console = AnsiConsole.Create(new()
{
    ColorSystem = ColorSystemSupport.Detect,
    Ansi = AnsiSupport.Detect,
    Enrichment = new() { UseDefaultEnrichers = true },
    Interactive = InteractionSupport.Detect
});

var configsManagerFactory = () => new JsonConfigsManager();
var fs = new SystemFileManager();

var configs = new AppConfigsManager(Path.Combine(fs.Root, "Configs"), "Nipah-Text-Generator-CLI", configsManagerFactory, fs);
var localization = configs.GetSection("Localization");

var menu = new SelectionPrompt<string>();