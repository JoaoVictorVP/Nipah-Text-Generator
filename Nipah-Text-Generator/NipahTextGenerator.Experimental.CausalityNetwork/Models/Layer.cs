using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextGenerator.Experimental.CausalityNetwork.Models;

public class Layer
{
    public List<NeuronConnection> Connections { get; set; } = new(32);

    public void Reinforce(Neuron connection, double bias)
    {
        var conIdx = Connections.FindIndex(x => x.To == connection);
        if (conIdx < 0)
            Connections.Add(new(connection, bias));
        else
        {
            var curCon = Connections[conIdx];
            Connections[conIdx] = curCon with { Weight = curCon.Weight + bias };
        }
    }
}
