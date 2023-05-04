using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GenenticAlgorithmBlazor.Shared;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.Controller.Models;
using GeneticAlgorithm.FitnessFunctions.Interfaces;
using FitnessFunctionn = GeneticAlgorithm.FitnessFunctions.FitnessFunction;
using ISimulation = GeneticAlgorithm.Controller.ISimulation;

namespace Adapters
{
    public class AdapterFitnessFunction:IFitnessFunction
    {
        private readonly FitnessFunctionn fitnessFunction;

        public AdapterFitnessFunction(FitnessFunctionn fitnessFunction,string name)
        {
            this.fitnessFunction = fitnessFunction;
            Name = name;
        }
        public Task<ISimulation> GetRequiredSimulation(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Type GetRequiredSimulationType()
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<Chromosome,float>> GetFitnessAsync(List<Chromosome> chromosomes, CancellationToken token)
        {
            var fitnessChromosomes = new List<IChromosome>();
            foreach (var c in chromosomes)
            {
                fitnessChromosomes.Add(new SharedChromosome(c));
            }
            var scores =await fitnessFunction.CalculateFitnessAsync(fitnessChromosomes, token);
            var returnValues = new Dictionary<Chromosome, float>();
            foreach (var s in scores)
            {

                Chromosome c=null;
                foreach (var VARIABLE in chromosomes)
                {
                    if (VARIABLE.ID == s.Key.ID)
                    {
                        c = VARIABLE;
                    }
                }
                returnValues.Add(c,s.Value);
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

        public string Name { get; set; }
    }
    
}