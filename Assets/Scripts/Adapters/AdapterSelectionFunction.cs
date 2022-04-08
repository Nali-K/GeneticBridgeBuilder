using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.SelectionFunctions;
using GeneticAlgorithm.SelectionFunctions.Interfaces;

namespace Adapters
{
    public class AdapterSelectionFunction:GeneticAlgorithm.Controller.ISelectionFunction
    {
        private readonly SelectionFunction selectionFunction;

        public AdapterSelectionFunction(SelectionFunction function)
        {
            selectionFunction = function;
        }
        public bool GetExclusive()
        {
            return false;
        }

        public async Task<List<GeneticAlgorithm.Controller.Chromosome>> SelectChromosome(Dictionary<GeneticAlgorithm.Controller.Chromosome, ChromosomeScores> scores, CancellationToken token)
        {
            var chromosomes = new List<IChromosome>();
            foreach (var i1 in scores)
            {
                chromosomes.Add( new AdapterSelectionChromosome(i1.Value,i1.Key));
            }

            var output = selectionFunction.SelectChromosomes(chromosomes);
            var returnlist = new List<GeneticAlgorithm.Controller.Chromosome>();
            foreach(var chromosome in output){
                returnlist.Add((chromosome as AdapterSelectionChromosome).GetChromosome());
            }

            return returnlist;
        }



        public string ToJson()
        {
            throw new System.NotImplementedException();
        }

        public bool FromJson(string json)
        {
            throw new System.NotImplementedException();
        }

        public bool GetUnique()
        {
            throw new System.NotImplementedException();
        }
    }
}