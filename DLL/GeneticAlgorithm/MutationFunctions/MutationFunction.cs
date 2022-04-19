using System.Collections.Generic;
using GeneticAlgorithm.MutationFunctions.Interfaces;
namespace GeneticAlgorithm.MutationFunctions
{
    public abstract class MutationFunction
    {
        public abstract List<IChromosome> Mutate(List<IChromosome> c);
    }
}