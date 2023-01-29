using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeneticAlgorithm.FitnessFunctions.Interfaces
{
    public interface ISimulation
    {
        Task<bool> SimulateAsync(List<IChromosome> chromosomes);
    }
}