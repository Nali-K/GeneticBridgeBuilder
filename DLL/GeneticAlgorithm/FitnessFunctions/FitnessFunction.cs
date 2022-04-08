using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.FitnessFunctions.Interfaces;

namespace GeneticAlgorithm.FitnessFunctions
{
    public abstract class FitnessFunction
    {
        public abstract Task<float> CalculateFitness(IChromosome chromosome, CancellationToken token);
    }
}