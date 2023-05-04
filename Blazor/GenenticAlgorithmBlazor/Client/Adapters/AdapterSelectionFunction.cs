using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using GenenticAlgorithmBlazor.Client.Controllers;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.Controller.Models;
using GeneticAlgorithm.SelectionFunctions;
using ControllerChromosome=GeneticAlgorithm.Controller.Models.Chromosome;
using ISelectionFunction = GeneticAlgorithm.SelectionFunctions.ISelectionFunction;
using ControllerISelectionFunction = GeneticAlgorithm.Controller.ISelectionFunction;
using SelectionChromosome=GeneticAlgorithm.SelectionFunctions.Interfaces.IChromosome;
namespace Adapters
{
    public class AdapterSelectionFunction:ControllerISelectionFunction
    {
        private readonly ISelectionFunction _selectionFunction;
        private readonly EvolutionWorld _evolutionWorld;
        private Dictionary<string, int> _agents = new Dictionary<string, int>();

        public AdapterSelectionFunction(ISelectionFunction function,EvolutionWorld evolutionWorld)
        {
            _selectionFunction = function;
            _evolutionWorld = evolutionWorld;


        }
        public bool GetExclusive()
        {
            return false;
        }




        public async Task<List<ControllerChromosome>> SelectChromosomeAsync(List<Chromosome> population , CancellationToken token)
        {
            var chromosomes = new List<SelectionChromosome>();
            foreach (var chromosome in population)
            {
                chromosomes.Add( new AdapterSelectionChromosome(chromosome,_evolutionWorld));
            }

            var output = await _selectionFunction.SelectChromosomesAsync(chromosomes,token);
            
            var returnlist = new List<GeneticAlgorithm.Controller.Models.Chromosome>();
            var outputDistinct = output.Distinct();
            Console.Out.WriteLine("output:" + output.Count);
            Console.Out.WriteLine("output.Distinct:" + outputDistinct.Count());
            foreach(var chromosome in outputDistinct){
                if (chromosome.ChozenBy != null)
                {
                    var c = (chromosome as AdapterSelectionChromosome).GetChromosome();
                    //Console.Out.WriteLine("chozenby != null");
                    foreach (var agent in chromosome.ChozenBy)
                    {
                        Console.Out.WriteLine(agent);

                        var agentId = -1;
                        if (_agents.ContainsKey(agent))
                            agentId = _agents[agent];
                        else
                        {
                            agentId = _agents.Count;
                            _agents.Add(agent,agentId);
                        }

                        c.scores.ChozenBy ??= new List<int>();
                        c.scores.ChozenBy.Add(agentId);




                    }
                    returnlist.Add(c);
                }else
                {
                    Console.Out.WriteLine("chozenby == null");
                    returnlist.Add((chromosome as AdapterSelectionChromosome).GetChromosome());
                }

            }

            return returnlist;
        }

        public int GetNumberExpectedWinners()
        {
            return _selectionFunction.NumberWinning;
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