using System.Collections.Generic;
using System.Threading.Tasks;
using GeneticAlgorithm.MutationFunctions.Interfaces;
namespace GeneticAlgorithm.MutationFunctions
{
    public abstract class MutationFunction
    {
        public abstract Task<List<IChromosome>> MutateAsync(List<IChromosome> c);
    }
}