using System;
using System.Threading;
using System.Threading.Tasks;
using Simulation;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Genetics
{
    [Serializable] public class BridgeCheckFitness:FitnessFunction
    {
        public GameObject simulator;

        


        
        public BridgeCheckFitness(GameObject simulator)
        {
            this.simulator = simulator;

        }
        public override async Task<float> CalculateFitness(Chromosome c, CancellationToken token,Vector3 position)
        {
            var inst =Object.Instantiate(simulator,position,Quaternion.identity);
            var done = false;
            var simulation = inst.GetComponent<Simulation.Simulation>();
       
            var score = -1000;
            var weight = 6;
            var CalculatedCubeSpawnPos = false;
            Vector3 cubeSpawnPos= new Vector3();
            while (!done)
            {
                await Task.Delay(300,token);
                simulation.Simulate(c,position);
                await Task.Delay(100,token);
                if (!CalculatedCubeSpawnPos)
                {
                    cubeSpawnPos= await CalculateSpawnPos(position,token);
                    CalculatedCubeSpawnPos = true;
                }


                if (await runTest(weight,simulation,cubeSpawnPos,token))
                {
                    
                    if (score<0)
                    {
                        score = 1;
                    }
                    else
                    {
                        score += 1;
                    }
                    weight++;
                }
                else
                {
                    done = true;
                }

                if (weight > 6)
                {
                    done = true;
                }
                simulation.EndSimulation();
            }
            await Task.Delay(100,token);
            GameObject.Destroy(simulation.gameObject);
            
            return score;
        }

        private async Task<bool> runTest(int weight,Simulation.Simulation simulation,Vector3 cubeSpawnPos,CancellationToken token)
        {

            return await (simulation as BridgeSim).DropBlock(weight*weight, cubeSpawnPos,token);
        }
        private async Task<Vector3> CalculateSpawnPos(Vector3 space, CancellationToken token)
        {
            return space+new Vector3(4, 10, 4);
        }
    }
}