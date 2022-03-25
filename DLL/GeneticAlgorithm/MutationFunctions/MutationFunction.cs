using MutationFunctions.Interfaces;
namespace MutationFunctions
{
    public abstract class MutationFunction
    {
        public abstract IChromosome Mutate(IChromosome c);
    }
}