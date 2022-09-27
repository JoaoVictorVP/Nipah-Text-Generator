﻿
using NipahTextGenerator.Experimental.CausalityNetwork;
using NipahTextGenerator.Experimental.CausalityNetwork.Models;
using NipahTextGenerator.Syntactics;
using NipahTextGenerator.Syntactics.Models;
using Spectre.Console;

AnsiConsole.MarkupLine("Hello Semantics Trainer!");

void Menu(params (string name, Action action)[] options)
{
    var select = new SelectionPrompt<Action>()
        .Title("What you will do?");
    foreach (var option in options)
        select.AddChoice(option.action);
    select.UseConverter(act => Array.Find(options, x => x.action == act).name);
    var selected = AnsiConsole.Prompt(select);
    selected();
}

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

string[] SelectFiles()
{
    var files = Directory.EnumerateFiles("./sources");
    var selectFiles = new MultiSelectionPrompt<string>(StringComparer.Ordinal)
        .Title("Please select your files:");
    foreach(var file in files)
        selectFiles.AddChoice(file);
    var select = AnsiConsole.Prompt(selectFiles);
    return select.ToArray();
}

var ctx = new Context();

var options = new TrainerOptions(MaxDeep: 300, Bias: 0.03);

void DoTrain(string file)
{
    if (ctx is null || options is null)
        return;

    string text = File.ReadAllText(file);

    var trainer = new Trainer();

    Parallel.For(0, 3, _ =>
    {
        trainer.Train(ctx, text, options);
    });

    //for (int i = 0; i < 15; i++)
        trainer.Train(ctx, text, options);

    AnsiConsole.MarkupLine("Done!");
}

void Train()
{
    string file = SelectFile();

    if (file is null or "" || File.Exists(file) is false)
        return;

    DoTrain(file);
}

void TrainMulti()
{
    string[] files = SelectFiles();

    foreach (var file in files)
        if (file is null or "" || File.Exists(file) is false)
            return;

    Parallel.ForEach(files, DoTrain);
}

void Generate()
{
    var generator = new Generator();

    string text = generator.Generate(ctx);

    AnsiConsole.MarkupLine(text);

    AnsiConsole.WriteLine();

    AnsiConsole.MarkupLine("Generated!");
}

while (true)
{
    Menu(("Train", Train), ("Generate", Generate), ("Train Parallel", TrainMulti));
}
