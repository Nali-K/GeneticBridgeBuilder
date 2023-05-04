using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller;
<<<<<<< HEAD:Unity/GeneticBridgeBuilding/cSharpApp/BlazorGeneticAlgorithm/BlazorGeneticAlgorithm/Adapters/AdapterMutationFunction.cs

=======
using UnityEngine;
>>>>>>> 8cb50db32937e524d8bd2d753bfac97bb3eece2e:Assets/Scripts/Adapters/AdapterMutationFunction.cs
using GeneticAlgorithm.MutationFunctions;
using ControllerChromosome=GeneticAlgorithm.Controller.Chromosome;
using MutationChromosome=GeneticAlgorithm.MutationFunctions.Interfaces.IChromosome;
namespace Adapters
{
    public class AdapterMutationFunction : IMutationFunction

    {
        private MutationFunction mutationFunction;

        public AdapterMutationFunction(MutationFunction mutationFunction)
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

        public async Task<List<ControllerChromosome>> MutateAsync(List<ControllerChromosome> chromosome, CancellationToken token)
        {
            var mutationChromosomes = new List<MutationChromosome>();
            foreach (var c in chromosome)
            {
                mutationChromosomes.Add(new AdapterMutationChromosome(c));
            }
<<<<<<< HEAD:Unity/GeneticBridgeBuilding/cSharpApp/BlazorGeneticAlgorithm/BlazorGeneticAlgorithm/Adapters/AdapterMutationFunction.cs

            var mutated = await mutationFunction.MutateAsync(mutationChromosomes);

            var returnlist = new List<ControllerChromosome>();

=======
            Debug.Log("done");
            var mutated = await mutationFunction.MutateAsync(mutationChromosomes);
            Debug.Log("done");
            var returnlist = new List<ControllerChromosome>();
            Debug.Log("done");
>>>>>>> 8cb50db32937e524d8bd2d753bfac97bb3eece2e:Assets/Scripts/Adapters/AdapterMutationFunction.cs
            foreach (var m in mutated)
            {
                
                returnlist.Add((m as AdapterMutationChromosome).chromosome);
<<<<<<< HEAD:Unity/GeneticBridgeBuilding/cSharpApp/BlazorGeneticAlgorithm/BlazorGeneticAlgorithm/Adapters/AdapterMutationFunction.cs
            } 
=======
            } Debug.Log("done");
>>>>>>> 8cb50db32937e524d8bd2d753bfac97bb3eece2e:Assets/Scripts/Adapters/AdapterMutationFunction.cs
            return returnlist;
        }
    }
}