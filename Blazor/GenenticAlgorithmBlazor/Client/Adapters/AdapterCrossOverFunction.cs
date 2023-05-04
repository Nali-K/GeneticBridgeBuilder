using System;
using System.Collections.Generic;
using System.Linq;
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
        private ConsoleController _consoleController;
        private int _repeats;
        public AdapterCrossOverFunction(CrossOverFunction function,EvolutionWorld evolutionWorld, ConsoleController consoleController, int repeats=1)
        {
            crossOverFunction = function;
            _evolutionWorld = evolutionWorld;
            _consoleController = consoleController;
            _repeats = repeats;
        }

        public async Task<List<ControllerChromosome>> CrossOverAsync(List<ControllerChromosome> chromosomes, CancellationToken token)
        {
            var chrom = new List<AdapterCrossOverChromosome>();
            var co= chromosomes.Distinct().ToList();
            Console.Out.WriteLine("chromosomons legnth: "+ chromosomes.Count+" chromosomes distinct lenght: "+co.Count);
            for (int i = 0; i < co.Count(); i++)
            {
                if (co[i].scores is {ChozenBy: {Count: > 0}})
                {
                    foreach (var score in co[i].scores.ChozenBy)
                    {
                        chrom.Add(new AdapterCrossOverChromosome(co[i], _evolutionWorld, score,
                            _consoleController));
                    }

                    continue;
                }

                chrom.Add(new AdapterCrossOverChromosome(co[i],_evolutionWorld,-1,_consoleController)); 


            }
            var returnList = new List<ControllerChromosome>();
            var chromosomeArray = chrom.ToArray();
            for (var i = 0; i < _repeats; i++)
            {
                var newChromosomes=  await crossOverFunction.CrossOverAsync(chromosomeArray,token);



                foreach (var c in newChromosomes)
                {
                    returnList.Add((c as AdapterCrossOverChromosome).chromosome);
                }
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