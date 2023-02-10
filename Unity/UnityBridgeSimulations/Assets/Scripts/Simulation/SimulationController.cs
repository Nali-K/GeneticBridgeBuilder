using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Controllers;
using Enums;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Simulation
{
    public class SimulationController:MonoBehaviour
    {

        private List<Chromosome> chromosomes;
        private CancellationTokenSource tokenSource;

        public GameObject bridgeDropSimulationPrefab;
        public GameObject bridgeStabilitySimulationPrefab;
        public GameObject bridgePicturePrefab;
        public void BridgeSim(List<Chromosome> chromosomes)
        {
            this.chromosomes = chromosomes;
            var  tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
        }

        private void Start()
        {

            /*chromosomes = new List<Chromosome>();
            CreateChromosome();
            CreateChromosome();
            CreateChromosome();
            Debug.Log(chromosomes.Count);
            SaveIntoJson();
            HandleAssignments(tokenSource.Token);*/
        }


        public async Task<bool> RunAssignmentAsync(Assignment assignment,CancellationToken token)
        {
            foreach (var sim in assignment.simulationsToRun)
            {
                switch (Assignment.GetSimulation(sim))
                {
                    case Simulations.Dropblock:
                        await RunSimulationAsync(bridgeDropSimulationPrefab,assignment.chromosomes,token);
                        break;
                    case Simulations.Stability:
                        await RunSimulationAsync(bridgeStabilitySimulationPrefab,assignment.chromosomes,token);
                        break;
                    case Simulations.Picture:
                        await RunSimulationAsync(bridgePicturePrefab,assignment.chromosomes,token);
                        break;
                    default:
                        return false;
                }
                
            }

            return true;

        }
        private async Task testAsync(int delay,CancellationToken token)
        {
            await Task.Delay(delay *1000);
        }
        private async Task RunSimulationAsync(GameObject prefab,List<Chromosome> chromosomes,CancellationToken token)
        {
            var targetNumSimulations = 80;
            var distanceBetweenSimulations = 30;
            var numSimulations = chromosomes.Count < targetNumSimulations ? chromosomes.Count : targetNumSimulations;
            //Time.timeScale = 1f;
            var tasks = new List<Task>();
            var atChromosome = 0;
           
            //Physics.gravity = new Vector3(0, -0.9f, 0);
            var simulations = InitSimulations(numSimulations,distanceBetweenSimulations,prefab);

            await Task.Delay(100, token);
            
            for (var i = 0; i < numSimulations; i++)
            {
                //if (simulations[j].)
                tasks.Add(simulations[i].Simulate(chromosomes[atChromosome],new Vector3(0, 0, i*distanceBetweenSimulations), token));
                await Task.Delay(200, token);
              
                atChromosome++;
                
            }

            while (atChromosome<chromosomes.Count)
            {
                await Task.WhenAny(tasks);
                for (var i = 0; i < numSimulations; i++)
                {
                    if (tasks[i].IsCompleted)
                    {
                        tasks[i] = simulations[i].Simulate(chromosomes[atChromosome], new Vector3(0, 0, i*distanceBetweenSimulations), token);

                        atChromosome++;
                    }

                }
                

            }

            await Task.WhenAll(tasks);
            CloseSimulations(simulations);

        }

        private Simulation[] InitSimulations(int numSimulations,int distanceBetweenSimulations, GameObject prefab)
        {
            var simulations = new Simulation[numSimulations];
            for (var i = 0; i < numSimulations; i++)
            {
                var instance = Instantiate(prefab, new Vector3(0, 0, i*distanceBetweenSimulations), Quaternion.identity);
                simulations[i]=(instance.GetComponent<Simulation>());
            }

            return simulations;
        }

        private void CloseSimulations(List<Simulation> simulations)
        {

            foreach (var simulation in simulations)
            {
                simulation.EndSimulation();
                Destroy(simulation.gameObject);
            }
        }
        private void CloseSimulations(Simulation[] simulations)
        {

            foreach (var simulation in simulations)
            {
                simulation.EndSimulation();
                Destroy(simulation.gameObject);
            }
        }


        public void SaveIntoJson()
        {

            var c = JsonConvert.SerializeObject(chromosomes);
            Debug.Log(c);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/chromosomes.json", c);
            
        }
        public void CreateChromosome()
        {
            var values = new int[8, 5, 8];
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    for (var k = 0; k < 8; k++)
                    {
                        values[i, j, k] = Random.Range(0, 2);
                    }
                }
            }
            chromosomes.Add(new Chromosome(values));
        }
    }


}