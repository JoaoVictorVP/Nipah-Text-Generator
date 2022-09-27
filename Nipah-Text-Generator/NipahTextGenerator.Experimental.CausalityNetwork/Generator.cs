using NipahTextGenerator.Experimental.CausalityNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextGenerator.Experimental.CausalityNetwork;

public class Generator
{
    public string Generate(Context ctx)
    {
        var sb = new StringBuilder(3200);
        var rand = ctx.Rand;
        var itctx = ctx.GetIterator();

        var neuron = ctx.GetRandom();

        if (neuron is null) return "";

        sb = Drive(sb, neuron, itctx, rand);

        return sb.ToString();
    }
    StringBuilder Drive(StringBuilder sb, Neuron neuron, Context.IteratorContext itctx, Random rand)
    {
        sb.Append(neuron.Expression);
        sb.Append(' ');

        var iterator = itctx.GetOrCreateIteratorFor(neuron);
        var layer = iterator.GetNextIfExist();

        if(layer is not null)
        foreach (var con in layer.Connections)
        {
            if(rand.NextDouble() < con.Weight)
            {
                Drive(sb, con.To, itctx, rand);
                return sb;
            }
        }

        return sb;
    }
}
