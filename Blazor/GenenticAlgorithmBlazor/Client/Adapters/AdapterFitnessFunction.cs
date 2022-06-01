using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.FitnessFunctions.Interfaces;
using FitnessFunctionn = GeneticAlgorithm.FitnessFunctions.FitnessFunction;
using ISimulation = GeneticAlgorithm.Controller.ISimulation;

namespace Adapters
{
    public class AdapterFitnessFunction:IFitnessFunction
    {
        private readonly FitnessFunctionn fitnessFunction;

        public AdapterFitnessFunction(FitnessFunctionn fitnessFunction)
        {
            this.fitnessFunction = fitnessFunction;
        }
        public Task<ISimulation> GetRequiredSimulation(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Type GetRequiredSimulationType()
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary< GeneticAlgorithm.Controller.Chromosome,float>> GetFitnessAsync(List<GeneticAlgorithm.Controller.Chromosome> chromosomes, CancellationToken token)
        {
            var fitnessChromosomes = new List<IChromosome>();
            foreach (var c in chromosomes)
            {
                fitnessChromosomes.Add(new AdapterFitnessChromosome(c));
            }
            var scores =await fitnessFunction.CalculateFitnessAsync(fitnessChromosomes, token);
            var returnValues = new Dictionary<GeneticAlgorithm.Controller.Chromosome, float>();
            foreach (var s in scores)
            {

                returnValues.Add((s.Key as AdapterFitnessChromosome).GetChromosome(),s.Value);
            }

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