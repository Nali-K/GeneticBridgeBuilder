using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.FitnessFunctions.Interfaces;

namespace GeneticAlgorithm.FitnessFunctions
{
    public class BridgeStabilityFitness:FitnessFunction
    {


        
        public BridgeStabilityFitness()
        {
            //this.simulator = simulator;

        }
        public override async Task<Dictionary<IChromosome,float>> CalculateFitnessAsync(List<IChromosome> chromosomes, CancellationToken token)
        {
            var outputDict = new Dictionary<IChromosome, float>();
            /*var inst =Object.Instantiate(simulator,position,Quaternion.identity);

            var simulation = inst.GetComponent<Simulation.Simulation>();
            
            

            
            await Task.Delay(200,token);
            simulation.Simulate(c,position);
            await Task.Delay(200,token);


            var score = 5-await runTest(simulation,token);


            simulation.EndSimulation();

            GameObject.Destroy(simulation.gameObject);
            var f = 0.1f;*/
            foreach (var chromosome in chromosomes)
            {
                var fitness = await GetFitness(chromosome, token);
                outputDict.Add(chromosome,fitness);
            }

            return outputDict;
        }

        private async Task<float> GetFitness(IChromosome chromosome, CancellationToken token)
        {
            var array = chromosome.geneArray;
            var score=0f;
            foreach (var fl in array)
            {
                score += (fl - 0.5f);
            }

            return score;
        }
    }
}