using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.SelectionFunctions;
using ControllerChromosome=GeneticAlgorithm.Controller.Chromosome;
using SelectionChromosome=GeneticAlgorithm.SelectionFunctions.Interfaces.IChromosome;
namespace Adapters
{
    public class AdapterSelectionFunction:ISelectionFunction
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

        public async Task<List<ControllerChromosome>> SelectChromosomeAsync(Dictionary<ControllerChromosome, ChromosomeScores> scores, CancellationToken token)
        {
            var chromosomes = new List<SelectionChromosome>();
            foreach (var score in scores)
            {
                chromosomes.Add( new AdapterSelectionChromosome(score.Value,score.Key));
            }

            var output = await selectionFunction.SelectChromosomesAsync(chromosomes);
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