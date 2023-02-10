using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GenenticAlgorithmBlazor.Shared;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.Controller.Models;
using GeneticAlgorithm.FitnessFunctions.Interfaces;
using IChromosome = GeneticAlgorithm.VisualisationFunctions.Interfaces.IChromosome;
using VisualisationFunction = GeneticAlgorithm.VisualisationFunctions.VisualisationFunction;
using ISimulation = GeneticAlgorithm.Controller.ISimulation;
namespace Adapters
{
    public class AdapterVisualisationFunction:IVisualisationFunction
    {

        private readonly VisualisationFunction visualisationFunction;

        public AdapterVisualisationFunction(VisualisationFunction visualisationFunction)
        {
            this.visualisationFunction = visualisationFunction;
        }
        public Task<ISimulation> GetRequiredSimulation(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Type GetRequiredSimulationType()
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<Chromosome, List<ChromosomeVisualisation>>> GetVisualisationsAsync(List<Chromosome> chromosome, CancellationToken token)
        {
            var visualisationChromosomes = new List<IChromosome>();
            foreach (var c in chromosome)
            {
                visualisationChromosomes.Add(new SharedChromosome(c));
            }
            var results =await visualisationFunction.VisualiseAsync(visualisationChromosomes, token);
            var returnValues = new Dictionary<GeneticAlgorithm.Controller.Models.Chromosome, List<ChromosomeVisualisation>>();
            foreach (var r in results)
            {
                var cv=new List<ChromosomeVisualisation>();
                foreach (var d in r.Value)
                {
                    cv.Add(new ChromosomeVisualisation(d.Value,this,d.Key));
                }


                foreach (var c in chromosome)
                {
                    if(Enumerable.SequenceEqual(c.dimensionSize, r.Key.dimensionSize))
                        if (Enumerable.SequenceEqual(c.geneArray, r.Key.geneArray))
                        {
                            if (!returnValues.ContainsKey(c))
                                returnValues.Add(c,cv);
                        }
                }

                //var c = new Chromosome(s.Key.dimensionSize.Clone() as int []);

            }
            Console.WriteLine(returnValues.Keys.Count);
            Console.WriteLine(returnValues.Keys.Count);
            return returnValues;
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public bool FromJson(string json)
        {
            throw new NotImplementedException();
        }
    }
}