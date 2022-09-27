using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextTools.Cli.Settings;

public class FileSettings : CommandSettings
{
    [CommandArgument(0, "<file>")]
    public string File { get; set; } = "";
}
