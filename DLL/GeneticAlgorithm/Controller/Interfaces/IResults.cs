using System.Collections.Generic;

namespace GeneticAlgorithm.Controller
{
    public interface IResults
    {
        ISimulationResult GetResult(int numChromosome, ISimulation simulation);
    }
}