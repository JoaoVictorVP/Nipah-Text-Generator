using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextGenerator.Experimental.CausalityNetwork.Models;

public class Neuron
{
    public string Expression { get; set; } = "";
    public List<Layer> Layers { get; set; } = new(32);

    public Neuron(string expression)
    {
        Expression = expression;
    }
}
