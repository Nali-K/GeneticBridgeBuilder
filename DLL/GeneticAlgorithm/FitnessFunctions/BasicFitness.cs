using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.FitnessFunctions.Interfaces;
namespace GeneticAlgorithm.FitnessFunctions
{
    public class BasicFitness:FitnessFunction
    {
        public override async Task<Dictionary<IChromosome,float>> CalculateFitnessAsync(List<IChromosome> chromosomes, CancellationToken token)
        {
            var outputDict = new Dictionary<IChromosome, float>();
            foreach (var chromosome in chromosomes)
            {
                var fitness = await GetFitness(chromosome, token);
                outputDict.Add(chromosome,fitness);
                await Task.Delay(5,token);
            }

            return outputDict;
        }

        private async Task<float> GetFitness(IChromosome chromosome,CancellationToken token)
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