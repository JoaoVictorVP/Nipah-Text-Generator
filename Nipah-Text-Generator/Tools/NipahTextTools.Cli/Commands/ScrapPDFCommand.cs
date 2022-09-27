using NipahTextTools.Cli.Settings;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextTools.Cli.Commands;

public class ScrapPDFCommand : Command<ScrapPDFSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] ScrapPDFSettings settings)
    {

        return 0;
    }
}
