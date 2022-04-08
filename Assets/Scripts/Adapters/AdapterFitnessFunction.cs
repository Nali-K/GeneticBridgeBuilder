using System;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller;
using FitnessFunctionn = GeneticAlgorithm.FitnessFunctions.FitnessFunction;

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

        public Task<float> GetFitness(GeneticAlgorithm.Controller.Chromosome chromosome, CancellationToken token)
        {
            return fitnessFunction.CalculateFitness(new AdapterFitnessChromosome(chromosome), token);
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