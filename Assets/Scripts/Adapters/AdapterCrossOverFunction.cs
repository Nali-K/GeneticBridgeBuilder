using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.CrossOverFunctions;
namespace Adapters
{
    public class AdapterCrossOverFunction:ICrossOverFunction
    {
        private CrossOverFunction crossOverFunction;
        public AdapterCrossOverFunction(CrossOverFunction function)
        {
            crossOverFunction = function;
        }

        public async Task<GeneticAlgorithm.Controller.Chromosome> CrossOver(GeneticAlgorithm.Controller.Chromosome[] chromosomes, CancellationToken token)
        {
            var chrom = new AdapterCrossOverChromosome[chromosomes.Length];
            for (int i = 0; i < chromosomes.Length; i++)
            {
                chrom[i] = new AdapterCrossOverChromosome(chromosomes[i]);
            }
            var newChromosome=  crossOverFunction.CrossOver(chrom) as AdapterCrossOverChromosome;
            return newChromosome.chromosome;
        }

        public string ToJson()
        {
            throw new System.NotImplementedException();
        }

        public bool FromJson(string json)
        {
            throw new System.NotImplementedException();
        }
    }
}