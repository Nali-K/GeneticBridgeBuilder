using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.FitnessFunctions.Interfaces;

namespace GeneticAlgorithm.FitnessFunctions
{
   [Serializable] public class BridgeCheckFitness:FitnessFunction
    {
        public IBridgeCaryWeightSimulation simulator;

        


        
        public BridgeCheckFitness(IBridgeCaryWeightSimulation simulator)
        {
            this.simulator = simulator;

        }
        
        public override async Task<Dictionary<IChromosome,float>> CalculateFitnessAsync(List<IChromosome> chromosomes, CancellationToken token)
        {/*
            var inst =Object.Instantiate(simulator,position,Quaternion.identity);
            var done = false;
            var simulation = inst.GetComponent<Simulation.Simulation>();
       */
            var outputDict = new Dictionary<IChromosome, float>();
            var score = -1000;
            await simulator.SimulateAsync(chromosomes);

            foreach (var chromosome in chromosomes)
            {
                var weight = simulator.GetMaxWeight(chromosome);
    /*            if (weight < 1)
                {
                    outputDict.Add(chromosome,-20);
                }
                else
                {*/
                    outputDict.Add(chromosome,-weight*8);
                //}

            }
            /*var weight = 4;
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

                if (weight > 4)
                {
                    done = true;
                }
                simulation.EndSimulation();
            }
            await Task.Delay(100,token);
            GameObject.Destroy(simulation.gameObject);
            */
            return outputDict;
        }


    }
}