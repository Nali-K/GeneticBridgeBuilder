using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller.Models;

namespace GeneticAlgorithm.Controller
{
    public interface IFitnessFunction
    { 
        Task<ISimulation> GetRequiredSimulation(CancellationToken token);
        Type GetRequiredSimulationType();
        Task<Dictionary<Chromosome,float>> GetFitnessAsync(List<Chromosome> chromosome,CancellationToken token);
        string Name { get; set; }
        string ToJson();
        bool FromJson(string json);
    }
}