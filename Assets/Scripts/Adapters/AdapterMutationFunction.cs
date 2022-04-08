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

        public async Task<GeneticAlgorithm.Controller.Chromosome> Mutate(GeneticAlgorithm.Controller.Chromosome chromosome, CancellationToken token)
        {
            return (mutationFunction.Mutate(new AdapterMutationChromosome(chromosome)) as AdapterMutationChromosome).chromosome;
        }
    }
}