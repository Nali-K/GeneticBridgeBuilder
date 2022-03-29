using System.Threading;
using System.Threading.Tasks;
using FitnessFunctions.Interfaces;

namespace FitnessFunctions
{
    public class BridgeStabilityFitness:FitnessFunction
    {


        
        public BridgeStabilityFitness()
        {
            //this.simulator = simulator;

        }
        public async Task<float> CalculateFitness(IChromosome chromosome, CancellationToken token)
        {
            /*var inst =Object.Instantiate(simulator,position,Quaternion.identity);

            var simulation = inst.GetComponent<Simulation.Simulation>();
            
            


            await Task.Delay(200,token);
            simulation.Simulate(c,position);
            await Task.Delay(200,token);


            var score = 5-await runTest(simulation,token);


            simulation.EndSimulation();

            GameObject.Destroy(simulation.gameObject);
            var f = 0.1f;*/
            return score;
        }

    }
}