using NipahTextGenerator.Experimental.CausalityNetwork.Models;
using NipahTokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextGenerator.Experimental.CausalityNetwork;

public class Trainer
{
    public void Train(Context ctx, string text, TrainerOptions options)
    {
        var tokens = Common.Tokenizer.Tokenize(text, Common.TokenizerOptions, new LocalStringBuilderPool());
        //var tokens = new List<Token>(text.Length);
        //foreach (var c in text) tokens.Add(Token.BuildRaw(new SplitItem(c.ToString(), 0, 0)));
        var itctx = ctx.GetIterator();
        DoTrain(ctx, itctx, CollectionsMarshal.AsSpan(tokens), options, options.MaxDeep, null);
    }
    void DoTrain(Context ctx, Context.IteratorContext itctx, ReadOnlySpan<Token> tokens, TrainerOptions options, int deepCounter, Neuron? carry)
    {
        if (tokens.IsEmpty is true || deepCounter <= 0)
            return;
        string exp = tokens[0].Text;
        var neuron = ctx.GetOrCreate(exp);
        if (carry is not null)
        {
            var iterator = itctx.GetOrCreateIteratorFor(carry);
            var layer = iterator.GetNext();
            layer.Reinforce(neuron, options.Bias);
        }
        DoTrain(ctx, itctx, tokens[1..], options, deepCounter - 1, neuron);
    }
}
