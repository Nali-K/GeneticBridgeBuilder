using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller;
using UnityEngine;

namespace Adapters
{
    public class AdapterMutationFunction : IMutationFunction

    {
        private GeneticAlgorithm.MutationFunctions.MutationFunction mutationFunction;

        public AdapterMutationFunction(GeneticAlgorithm.MutationFunctions.MutationFunction mutationFunction)
        {
            this.mutationFunction = mutationFunction;
        }
        public string ToJson()
        {
            throw new System.NotImplementedException();
        }

        public bool FromJson(string json)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<GeneticAlgorithm.Controller.Chromosome>> Mutate(List<GeneticAlgorithm.Controller.Chromosome> chromosome, CancellationToken token)
        {
            var mutationChromosomes = new List<GeneticAlgorithm.MutationFunctions.Interfaces.IChromosome>();
            foreach (var c in chromosome)
            {
                mutationChromosomes.Add(new AdapterMutationChromosome(c));
            }

            var mutated = mutationFunction.Mutate(mutationChromosomes);
            var returnlist = new List<GeneticAlgorithm.Controller.Chromosome>();
            foreach (var m in mutated)
            {
                
                returnlist.Add((m as AdapterMutationChromosome).chromosome);
            }
            return returnlist;
        }
    }
}