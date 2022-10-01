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

        sb = Drive(sb, neuron, itctx, rand, 1);

        return sb.ToString();
    }
    StringBuilder Drive(StringBuilder sb, Neuron neuron, Context.IteratorContext itctx, Random rand, double carry)
    {
        sb.Append(neuron.Expression);
        //if((neuron.Expression.Length is 1 && (char.IsSeparator(neuron.Expression[0]) || char.IsSymbol(neuron.Expression[0]))) is false)
            sb.Append(' ');

        var iterator = itctx.GetOrCreateIteratorFor(neuron);
        var layer = iterator.GetNextIfExist();

        if(layer is not null)
        foreach (var con in layer.Connections)
        {
            if(rand.NextDouble() / carry < con.Weight)
            {
                Drive(sb, con.To, itctx, rand, con.Weight);
                return sb;
            }
        }

        return sb;
    }
}
