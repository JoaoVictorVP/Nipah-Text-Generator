
using NipahTextGenerator.Experimental.CausalityNetwork;
using NipahTextGenerator.Experimental.CausalityNetwork.Models;
using NipahTextGenerator.Syntactics;
using NipahTextGenerator.Syntactics.Models;
using Spectre.Console;

Console.WriteLine("Hello Semantics Trainer!");

string SelectFile()
{
    var files = Directory.EnumerateFiles("./sources");
    var selectFile = new SelectionPrompt<string>()
        .Title("Please select your file:");
    foreach (var file in files)
        selectFile.AddChoice(file);
    var select = AnsiConsole.Prompt(selectFile);
    return select;
}

var ctx = new Context();

var options = new TrainerOptions(MaxDeep: 300, Bias: 0.03);

while (true)
{
    string file = SelectFile();

    if (file is null or "" || File.Exists(file) is false)
        return;

    string text = File.ReadAllText(file);

    var trainer = new Trainer();

    trainer.Train(ctx, text, options);

    AnsiConsole.MarkupLine("Done!");
}
