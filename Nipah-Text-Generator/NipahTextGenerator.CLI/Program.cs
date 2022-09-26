
using AppConfigsUtils;
using AppConfigsUtils.Impl;
using AppLocalizationUtils;
using NipahTextGenerator.CLI;
using Spectre.Console;

var cts = new CancellationTokenSource();
var ct = cts.Token;

var console = AnsiConsole.Create(new()
{
    ColorSystem = ColorSystemSupport.Detect,
    Ansi = AnsiSupport.Detect,
    Enrichment = new() { UseDefaultEnrichers = true },
    Interactive = InteractionSupport.Detect
});

const string defaultLocalization = "en-US";

var configsManagerFactory = () => new JsonConfigsManager();
var fs = new SystemFileManager();
var locMan = new LocalizationManager();

var localization = LocalizationLoader.Load(defaultLocalization) ?? throw new Exception("Can't find default localization");

var configs = new AppConfigsManager(Path.Combine(fs.Root, "Configs"), "Nipah-Text-Generator-CLI", configsManagerFactory, fs);
var locSection = await configs.GetSection("Localization")
    ?? throw new Exception("Problem trying to get localization configs");

await locSection.Hook<string>("Language", language =>
{
    if(language is not null)
        localization = LocalizationLoader.Load(language);
});

SelectionPrompt<string> GetMenu() => new SelectionPrompt<string>()
    .Title(localization!.Get("menu_title"))
    .AddChoices(localization!.Get("menu_options"));

var menu = GetMenu();

await menu.ShowAsync(console, ct);