using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.VisualisationFunctions.Interfaces;

namespace GeneticAlgorithm.VisualisationFunctions
{
    public abstract class VisualisationFunction
    {
        public abstract Task<Dictionary<IChromosome,Dictionary<string,string>>> VisualiseAsync(List<IChromosome> chromosomes, CancellationToken token);
    }
}