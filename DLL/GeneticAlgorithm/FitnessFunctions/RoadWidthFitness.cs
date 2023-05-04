using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.FitnessFunctions.Interfaces;

namespace GeneticAlgorithm.FitnessFunctions
{
    [Serializable] public class RoadWidthFitness:FitnessFunction
    {        
        public IBridgeRoadSimulation simulator;
        
        
        public RoadWidthFitness(IBridgeRoadSimulation simulator)
        {
            this.simulator = simulator;

        }
        public override async Task<Dictionary<IChromosome, float>> CalculateFitnessAsync(List<IChromosome> chromosomes, CancellationToken token)
        {
            var outputDict = new Dictionary<IChromosome, float>();

            await simulator.SimulateAsync(chromosomes);

            foreach (var chromosome in chromosomes)
            {
                var numRoads = simulator.GetNumRoads(chromosome);
                if (numRoads < 0.0001f)
                {
                    outputDict.Add(chromosome,-10);
                }
                else
                {
                    outputDict.Add(chromosome, (float)Math.Pow(numRoads,0.7)*5f);
                }

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