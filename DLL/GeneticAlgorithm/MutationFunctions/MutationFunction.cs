using GeneticAlgorithm.MutationFunctions.Interfaces;
namespace GeneticAlgorithm.MutationFunctions
{
    public abstract class MutationFunction
    {
        public abstract IChromosome Mutate(IChromosome c);
    }
}