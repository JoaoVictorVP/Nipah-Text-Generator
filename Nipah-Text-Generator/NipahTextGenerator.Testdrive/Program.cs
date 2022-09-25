
using NipahTextGenerator.Syntactics;
using NipahTextGenerator.Syntactics.Models;

Console.WriteLine("Hello Syntax Trainer!");

Console.WriteLine("Please type your text:");

string file = Console.ReadLine() ?? "";

if (file is null or "")
    return;

string text = File.ReadAllText(file);

var syntax = new Syntax("en-us");

var trainer = new SyntaxTrainer();

trainer.Train(text, syntax);

Console.WriteLine("See your trained syntax:");

Console.WriteLine(syntax);

Console.ReadKey(true);