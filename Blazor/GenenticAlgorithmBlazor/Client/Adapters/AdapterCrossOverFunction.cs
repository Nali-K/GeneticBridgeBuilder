using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.Controller.Models;
using GeneticAlgorithm.CrossOverFunctions;
using ControllerChromosome= GeneticAlgorithm.Controller.Models.Chromosome;
namespace Adapters
{
    public class AdapterCrossOverFunction:ICrossOverFunction
    {
        private CrossOverFunction crossOverFunction;
        private EvolutionWorld _evolutionWorld;
        public AdapterCrossOverFunction(CrossOverFunction function,EvolutionWorld evolutionWorld)
        {
            crossOverFunction = function;
            _evolutionWorld = evolutionWorld;
        }

        public async Task<List<ControllerChromosome>> CrossOverAsync(List<ControllerChromosome> chromosomes, CancellationToken token)
        {
            var chrom = new AdapterCrossOverChromosome[chromosomes.Count];
            for (int i = 0; i < chromosomes.Count; i++)
            {
                chrom[i] = new AdapterCrossOverChromosome(chromosomes[i],_evolutionWorld);
            }
            var newChromosomes=  await crossOverFunction.CrossOverAsync(chrom,token);

            var returnList = new List<ControllerChromosome>();

            foreach (var c in newChromosomes)
            {
                returnList.Add((c as AdapterCrossOverChromosome).chromosome);
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