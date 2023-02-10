using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using GeneticAlgorithm.Controller.Models;

namespace GeneticAlgorithm.Controller
{
    public interface IVisualisationFunction
    { 
        Task<ISimulation> GetRequiredSimulation(CancellationToken token);
        Type GetRequiredSimulationType();
        Task<Dictionary<Chromosome,List<ChromosomeVisualisation>>> GetVisualisationsAsync(List<Chromosome> chromosome,CancellationToken token);
        string ToJson();
        bool FromJson(string json);
    }
}