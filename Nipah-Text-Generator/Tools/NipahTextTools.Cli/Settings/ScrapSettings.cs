using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextTools.Cli.Settings;

public class ScrapSettings : CommandSettings
{
    [Description("File to scrap")]
    [CommandArgument(0, "<file>")]
    public string File { get; set; } = "";

    [Description("Output path of the scrap. Defaults to file directory at './scraps/{filename}.txt'")]
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
            string? inputDir = Path.GetDirectoryName(File);
            if (inputDir is null) return ValidationResult.Error("Can't get input file directory");
            string name = Path.GetFileNameWithoutExtension(File);
            string dir = Path.Combine(inputDir, "scraps");
            if (Directory.Exists(dir) is false) Directory.CreateDirectory(dir);
            Output = Path.Combine(dir, $"{name}.txt");
        }

        return base.Validate();
    }
}
