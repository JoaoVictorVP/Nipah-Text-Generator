using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextGenerator.Experimental.CausalityNetwork.Models;

public class Context
{
    public Random Rand { get; set; } = new();
    readonly Dictionary<string, Neuron> neurons = new(32);

    public Neuron GetOrCreate(string expression)
    {
        return neurons.TryGetValue(expression, out Neuron? neuron) 
            ? neuron 
            : neurons[expression] = new(expression);
    }

    public IteratorContext GetIterator()
        => new(this);

    public class IteratorContext
    {
        readonly Context ctx;
        readonly Dictionary<Neuron, NeuronIterator> iterators = new(32);

        public NeuronIterator GetOrCreateIteratorFor(Neuron neuron)
        {
            return iterators.TryGetValue(neuron, out var iterator)
                ? iterator
                : iterators[neuron] = new(neuron);
        }

        public IteratorContext(Context ctx)
        {
            this.ctx = ctx;
        }
    }
}