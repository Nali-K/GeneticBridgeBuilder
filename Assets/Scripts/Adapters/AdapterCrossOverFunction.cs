using System.Collections.Generic;
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

        public async Task<List<GeneticAlgorithm.Controller.Chromosome>> CrossOver(List<GeneticAlgorithm.Controller.Chromosome> chromosomes, CancellationToken token)
        {
            var chrom = new AdapterCrossOverChromosome[chromosomes.Count];
            for (int i = 0; i < chromosomes.Count; i++)
            {
                chrom[i] = new AdapterCrossOverChromosome(chromosomes[i]);
            }
            var newChromosomes=  crossOverFunction.CrossOver(chrom) as AdapterCrossOverChromosome[];
            var returnList = new List<GeneticAlgorithm.Controller.Chromosome>();
            foreach (var c in newChromosomes)
            {
                returnList.Add(c.chromosome);
            }
            return returnList;
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