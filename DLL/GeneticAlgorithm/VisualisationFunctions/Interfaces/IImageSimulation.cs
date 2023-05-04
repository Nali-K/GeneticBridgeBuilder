using System.Collections.Generic;

namespace GeneticAlgorithm.VisualisationFunctions.Interfaces
{
    public interface IImageSimulation:ISimulation
    {
        Dictionary<string,string> GetImages(IChromosome chromosomes);
    }
}