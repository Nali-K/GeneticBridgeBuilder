using System.Threading;
using System.Threading.Tasks;
using Simulation;
using UnityEngine;

namespace Genetics
{
    public class StabilityFitness:FitnessFunction
    {
        public GameObject simulator;
        /*public Simulation.Simulation simulation;
        public bool done = false;
        public float score = 0;
        public Vector3 cubeSpawnPos;
        public bool CalculatedCubeSpawnPos;
        public float weight;
*/
        
        public StabilityFitness(GameObject simulator)
        {
            this.simulator = simulator;

        }
        public override async Task<float> CalculateFitness(Chromosome c, CancellationToken token,UnityEngine.Vector3 position)
        {
            var inst =Object.Instantiate(simulator,position,Quaternion.identity);

            var simulation = inst.GetComponent<Simulation.Simulation>();
            
            


            await Task.Delay(200,token);
            simulation.Simulate(c,position);
            await Task.Delay(200,token);


            var score = 5-await runTest(simulation,token);


            simulation.EndSimulation();

            GameObject.Destroy(simulation.gameObject);
            var f = 0.1f;
            return score;
        }

        private async Task<float> runTest(Simulation.Simulation simulation,CancellationToken token)
        {
            return await (simulation as BridgeSim).GetStability(token);
        }
    }
}