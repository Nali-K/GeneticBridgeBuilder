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
        public void BridgeSim(List<Chromosome> chromosomes)
        {
            this.chromosomes = chromosomes;
            var  tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
        }

        private void Start()
        {
            chromosomes = new List<Chromosome>();
            CreateChromosome();
            CreateChromosome();
            CreateChromosome();
            Debug.Log(chromosomes.Count);
            SaveIntoJson();
            HandleAssignments(tokenSource.Token);
        }

        private async Task HandleAssignments(CancellationToken token)
        {
            assignment = await AssignmentLoader.LoadAssignmentAsync(token);
            await RunAssignmentAsync(token);
            //await AssignmentUploader.UploadAssignmentAsync(token);
            assignment = null;
        }

        private async Task RunAssignmentAsync(CancellationToken token)
        {
            foreach (var sim in assignment.simulationToRun)
            {
                switch (sim)
                {
                    case Simulations.Dropblock:
                        await RunSimulationAsync<BridgeDropSimulation>();
                        break;
                    case Simulations.Stability:
                        await RunSimulationAsync<BridgeStablilitySimulation>();
                        break;
                }
                
            }

            assignment.completed = true;
        }

        private async Task RunSimulationAsync<T>()
        {
            throw new NotImplementedException();
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