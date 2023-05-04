using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.CrossOverFunctions;
<<<<<<< HEAD:Unity/GeneticBridgeBuilding/cSharpApp/BlazorGeneticAlgorithm/BlazorGeneticAlgorithm/Adapters/AdapterCrossOverFunction.cs
=======
using UnityEngine;
>>>>>>> 8cb50db32937e524d8bd2d753bfac97bb3eece2e:Assets/Scripts/Adapters/AdapterCrossOverFunction.cs
using ControllerChromosome= GeneticAlgorithm.Controller.Chromosome;
namespace Adapters
{
    public class AdapterCrossOverFunction:ICrossOverFunction
    {
        private CrossOverFunction crossOverFunction;
        public AdapterCrossOverFunction(CrossOverFunction function)
        {
            crossOverFunction = function;
        }

        public async Task<List<ControllerChromosome>> CrossOverAsync(List<ControllerChromosome> chromosomes, CancellationToken token)
        {
            var chrom = new AdapterCrossOverChromosome[chromosomes.Count];
            for (int i = 0; i < chromosomes.Count; i++)
            {
                chrom[i] = new AdapterCrossOverChromosome(chromosomes[i]);
            }
            var newChromosomes=  await crossOverFunction.CrossOverAsync(chrom);
<<<<<<< HEAD:Unity/GeneticBridgeBuilding/cSharpApp/BlazorGeneticAlgorithm/BlazorGeneticAlgorithm/Adapters/AdapterCrossOverFunction.cs

            var returnList = new List<ControllerChromosome>();

=======
            Debug.Log("hier?");
            var returnList = new List<ControllerChromosome>();
            Debug.Log("of hier?");
            Debug.Log(newChromosomes.Length);
>>>>>>> 8cb50db32937e524d8bd2d753bfac97bb3eece2e:Assets/Scripts/Adapters/AdapterCrossOverFunction.cs
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