using System;
using System.Collections.Generic;

namespace GeneticAlgorithm.FitnessFunctions.Interfaces
{
    public interface ISimulation
    {
        List<Double> GetResults(IChromosome chromosome);
    }
}