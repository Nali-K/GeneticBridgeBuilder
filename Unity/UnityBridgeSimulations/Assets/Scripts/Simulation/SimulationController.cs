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
        public IAssignmentLoader AssignmentLoader;
        private Assignment assignment;
        private List<Chromosome> chromosomes;
        private CancellationTokenSource tokenSource;
        public CoroutineRunner coroutineRunner;
        public GameObject bridgeDropSimulationPrefab;
        public GameObject bridgeStabilitySimulationPrefab;
        public void BridgeSim(List<Chromosome> chromosomes)
        {
            this.chromosomes = chromosomes;
            var  tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
        }

        private void Start()
        {
            AssignmentLoader = new LoadAssigmentFromWebApi(coroutineRunner);
            /*chromosomes = new List<Chromosome>();
            CreateChromosome();
            CreateChromosome();
            CreateChromosome();
            Debug.Log(chromosomes.Count);
            SaveIntoJson();
            HandleAssignments(tokenSource.Token);*/
        }

        public async Task HandleAssignments(CancellationToken token)
        {
            assignment = await AssignmentLoader.LoadAssignmentAsync(token);
            await RunAssignmentAsync(token);
            //await AssignmentUploader.UploadAssignmentAsync(token);
            assignment = null;
        }

        private async Task RunAssignmentAsync(CancellationToken token)
        {
            foreach (var sim in assignment.simulationsToRun)
            {
                switch (Assignment.GetSimulation(sim))
                {
                    case Simulations.Dropblock:
                        await RunSimulationAsync(bridgeDropSimulationPrefab,token);
                        break;
                    case Simulations.Stability:
                        await RunSimulationAsync(bridgeStabilitySimulationPrefab,token);
                        break;
                }
                
            }

            assignment.completed = true;
            var submitter = new SubmitAssignmentToWebApi(coroutineRunner);
            var s=await submitter.SubmitAssignmentAsync(token, assignment);
        }

        private async Task testAsync(int delay,CancellationToken token)
        {
            await Task.Delay(delay *1000);
        }
        private async Task RunSimulationAsync(GameObject prefab,CancellationToken token)
        {
            var numSimulations = 20;
            var simulations = new Simulation[numSimulations];
            var tasks = new List<Task>();
            var atChromosome = 0;
            for (var i = 0; i < numSimulations; i++)
            {
                var instance = Instantiate(prefab, new Vector3(0, 0, i*20), Quaternion.identity);
                simulations[i]=(instance.GetComponent<Simulation>());
            }

            await Task.Delay(100, token);
            
            for (var i = 0; i < numSimulations; i++)
            {
                //if (simulations[j].)
                tasks.Add(simulations[i].Simulate(assignment.chromosomes[atChromosome],new Vector3(0, 0, i*20), token));
                await Task.Delay(200, token);
              
                atChromosome++;
                
            }

            while (atChromosome<assignment.chromosomes.Count)
            {
                await Task.WhenAny(tasks);
                for (var i = 0; i < numSimulations; i++)
                {
                    if (tasks[i].IsCompleted)
                    {
                        tasks[i] = simulations[i].Simulate(assignment.chromosomes[atChromosome], new Vector3(0, 0, i*20), token);

                        atChromosome++;
                    }

                }
                

            }

            await Task.WhenAll(tasks);
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