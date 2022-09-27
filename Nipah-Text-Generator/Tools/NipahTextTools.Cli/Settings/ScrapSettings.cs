using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextTools.Cli.Settings;

public class ScrapSettings : CommandSettings
{
    [CommandArgument(0, "<file>")]
    public string File { get; set; } = "";

    [CommandOption("-o|--output")]
    public string Output { get; set; } = "";

    public override ValidationResult Validate()
    {
        if (File?.Trim() is null or "")
            return ValidationResult.Error("File should not be null or empty");
        if (System.IO.File.Exists(File) is false)
            return ValidationResult.Error("File should exist");

        if (Output is "")
        {
            string name = Path.GetFileNameWithoutExtension(File);
            if (Directory.Exists("C:/scraps") is false) Directory.CreateDirectory("C:/scraps");
            Output = Path.Combine("C:/scraps", $"{name}.txt");
        }

        return base.Validate();
    }
}
