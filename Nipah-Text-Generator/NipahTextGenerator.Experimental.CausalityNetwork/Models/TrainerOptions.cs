using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NipahTextGenerator.Experimental.CausalityNetwork.Models;

public record TrainerOptions(int MaxDeep = 30, double Bias = 0.03);