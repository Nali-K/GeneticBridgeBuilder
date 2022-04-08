using System.CodeDom.Compiler;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.FitnessFunctions.Interfaces;
namespace GeneticAlgorithm.FitnessFunctions
{
    public class BasicFitness:FitnessFunction
    {
        public override async Task<float> CalculateFitness(IChromosome chromosome, CancellationToken token)
        {
            var score = 0f;
            var genes = chromosome.GetGeneArray();
            foreach (var t in genes)
            {
                score += t;
            }

            score /= genes.Length;

            return score*-50;
        }
    }
}