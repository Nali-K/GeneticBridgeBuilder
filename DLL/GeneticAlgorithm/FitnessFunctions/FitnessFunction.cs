using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.FitnessFunctions.Interfaces;

namespace GeneticAlgorithm.FitnessFunctions
{
    public abstract class FitnessFunction
    {
        public abstract Task<Dictionary<IChromosome,float>> CalculateFitnessAsync(List<IChromosome> chromosomes, CancellationToken token);
    }
}