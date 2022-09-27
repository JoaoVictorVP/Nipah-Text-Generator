using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextGenerator.Experimental.CausalityNetwork.Models;

public class NeuronIterator
{
    readonly Neuron neuron;
    int curIndex = -1;

    public NeuronIterator(Neuron neuron)
    {
        this.neuron = neuron;
    }

    public Layer GetNext()
    {
        curIndex++;
        if (neuron.Layers.Count <= curIndex)
        {
            var l = new Layer();
            neuron.Layers.Add(l);
            return l;
        }
        else
            return neuron.Layers[curIndex];
    }
}
