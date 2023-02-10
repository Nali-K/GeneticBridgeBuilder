using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeneticAlgorithm.VisualisationFunctions.Interfaces
{
    public interface ISimulation
    {
        Task<bool> SimulateAsync(List<IChromosome> chromosomes);
    }
}