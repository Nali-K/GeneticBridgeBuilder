using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using GenenticAlgorithmBlazor.Client.Controllers;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.Controller.Models;
using GeneticAlgorithm.SelectionFunctions;
using ControllerChromosome=GeneticAlgorithm.Controller.Models.Chromosome;
using SelectionChromosome=GeneticAlgorithm.SelectionFunctions.Interfaces.IChromosome;
namespace Adapters
{
    public class AdapterSelectionFunction:ISelectionFunction
    {
        private readonly SelectionFunction selectionFunction;
        private Dictionary<IFitnessFunction, string> fitnessFunctionNames;

        public AdapterSelectionFunction(SelectionFunction function,Dictionary<IFitnessFunction, string> fitnessFunctionNames)
        {
            selectionFunction = function;
            
            this.fitnessFunctionNames = fitnessFunctionNames;

        }
        public bool GetExclusive()
        {
            return false;
        }


        public async Task<List<ControllerChromosome>> SelectChromosomeAsync(List<ChromosomeScores> scores , CancellationToken token)
        {
            var chromosomes = new List<SelectionChromosome>();
            foreach (var score in scores)
            {
                chromosomes.Add( new AdapterSelectionChromosome(score,fitnessFunctionNames));
            }

            var output = await selectionFunction.SelectChromosomesAsync(chromosomes,token);
            var returnlist = new List<GeneticAlgorithm.Controller.Models.Chromosome>();
            foreach(var chromosome in output){
                returnlist.Add((chromosome as AdapterSelectionChromosome).GetChromosome());
            }

            return returnlist;
        }

        public int GetNumberExpectedWinners()
        {
            return selectionFunction.GetNumberExpectedWinners();
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