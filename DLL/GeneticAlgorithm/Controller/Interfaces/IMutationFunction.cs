﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller.Models;

namespace GeneticAlgorithm.Controller
{
    public interface IMutationFunction
    {
        string ToJson();
        bool FromJson(string json);
        Task<List<Chromosome>> MutateAsync(List<Chromosome> chromosome, CancellationToken token);
    }
}