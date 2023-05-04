using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Controllers;
using Enums;


using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace Simulation
{
    public class SimulationController:MonoBehaviour
    {

        private List<Chromosome> chromosomes;
        private CancellationTokenSource tokenSource;

        public GameObject bridgeDropSimulationPrefab;
        public GameObject bridgeStabilitySimulationPrefab;
        public GameObject bridgeRoadSimulationPrefab;
        public GameObject bridgePicturePrefab;
        private float[] performanceArray = new float[20];
        private int performanceItterator =0;
        public float averagePerf=1f;
        public float spike= 1f;
        public bool canSpawn = true;
        private Simulation[] simulations =Array.Empty<Simulation>();
        public Text spikeTxt;
        public Text averagePerfTxt;
        public Image RedBar;
        public Image YellowBar;
        public Image GreenBar;
        public bool Hide = false;
        public void BridgeSim(List<Chromosome> chromosomes)
        {
            this.chromosomes = chromosomes;
            var  tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
        }

        private void Start()
        {
            
            for (var i=0;i<20;i++)
            {
                performanceArray[i] = Time.deltaTime;
            }
            /*chromosomes = new List<Chromosome>();
            CreateChromosome();
            CreateChromosome();
            CreateChromosome();
            Debug.Log(chromosomes.Count);
            SaveIntoJson();
            HandleAssignments(tokenSource.Token);*/
        }

        public void Update()
        {
            performanceArray[performanceItterator] = Time.deltaTime;
            if (++performanceItterator == 20) performanceItterator = 0;
            spike = 0;
            averagePerf = 0;
            foreach (var performance in performanceArray)
            {
                if (performance > spike) spike = performance;
                averagePerf += performance;
                
            }
            canSpawn = true;
            averagePerf /= 20;
            spikeTxt.text = "Performance Spike:" + Mathf.RoundToInt(spike * 1000) + " ms";
            averagePerfTxt.text = "Average Performance:" + Mathf.RoundToInt(averagePerf * 1000) + "ms";
            if (simulations.Length > 0)
            {
                RedBar.enabled = true;
                YellowBar.enabled = true;
                GreenBar.enabled = true;
                var fractionBussy = (float) GetBussy() / simulations.Length;
                var fractionDone = (float) GetDone() / simulations.Length;
                GreenBar.rectTransform.offsetMax = new Vector2(fractionDone * 300, GreenBar.rectTransform.offsetMax.y);
                YellowBar.rectTransform.offsetMin = new Vector2(fractionDone * 300, YellowBar.rectTransform.offsetMin.y);
                YellowBar.rectTransform.offsetMax = new Vector2(fractionDone * 300+fractionBussy*300, YellowBar.rectTransform.offsetMax.y);
                
            }
            else
            {
                RedBar.enabled = false;
                YellowBar.enabled = false;
                GreenBar.enabled = false;
            }
        }

        private int GetDone()
        {
            var done = 0;

            foreach (var simulation in simulations)
            {
                if (simulation.done)
                {
                    done++;
                }
            }

            return done;

        }

        private int GetBussy()
        {
            var bussy = 0;

            foreach (var simulation in simulations)
            {
                if (simulation.started&&!simulation.done)
                {
                    bussy++;
                }
            }

            return bussy;
        }
        public async Task<bool> RunAssignmentAsync(Assignment assignment,CancellationToken token)
        {
            var chromosomes = new List<Chromosome>();
            foreach (var chromosome in assignment.chromosomes)
            {
                chromosomes.Add(chromosome.ToCromosome());
            }
            Debug.Log("runassignmentasync");
            foreach (var sim in assignment.simulationsToRun)
            {
                switch (Assignment.GetSimulation(sim))
                {
                    case Simulations.Dropblock:
                        await RunSimulationAsync(bridgeDropSimulationPrefab,chromosomes,token,assignment.id);
                        break;
                    case Simulations.Stability:
                        await RunSimulationAsync(bridgeStabilitySimulationPrefab,chromosomes,token,assignment.id);
                        break;
                    case Simulations.Road:
                        await RunSimulationAsync(bridgeRoadSimulationPrefab,chromosomes,token,assignment.id);
                        break;
                    case Simulations.Picture:
                        await RunSimulationAsync(bridgePicturePrefab,chromosomes,token,assignment.id);
                        break;
                    default:
                        return false;
                }
                
            }
            assignment.chromosomes.Clear();
            foreach (var chromosome in chromosomes)
            {
                assignment.chromosomes.Add(new JSonChromosome(chromosome));
            }
            //Debug.Log(JsonUtility.ToJson(assignment.chromosomes[0]));
            return true;


        }
        private async Task testAsync(int delay,CancellationToken token)
        {
            await Task.Delay(delay *1000);
        }
        private async Task RunSimulationAsync(GameObject prefab,List<Chromosome> chromosomes,CancellationToken token,int assignmentID)
        {
            var parts = Mathf.CeilToInt(chromosomes.Count / 150f);
            for (var i = 0; i < parts; i++)
            {
                List<Chromosome> subSetChromosomes;

                var index = i * 150;
                var count = 150;
                if (count + index > chromosomes.Count)
                {
                    count = chromosomes.Count - index;
                }

                subSetChromosomes = chromosomes.GetRange(index, count);



                //var targetNumSimulations = 150;
                var distanceBetweenSimulations = 50;
                var numSimulations =
                    subSetChromosomes.Count; // < targetNumSimulations ? chromosomes.Count : targetNumSimulations;
                //Time.timeScale = 1f;
                var tasks = new List<Task>();
                var atChromosome = 0;

                //Physics.gravity = new Vector3(0, -0.9f, 0);
                simulations = InitSimulations(numSimulations, distanceBetweenSimulations, prefab);

                await Task.Delay(100, token);

                for (var j = 0; j < numSimulations; j++)
                {
                    //if (simulations[j].)
                    tasks.Add(simulations[j].Simulate(subSetChromosomes[atChromosome],
                        new Vector3(0, 0, j * distanceBetweenSimulations), token, assignmentID, this, j * 120));


                    atChromosome++;

                }
/*
            while (atChromosome<chromosomes.Count)
            {
                await Task.WhenAny(tasks);
                for (var i = 0; i < numSimulations; i++)
                {
                    if (tasks[i].IsCompleted)
                    {
                        tasks[i] = simulations[i].Simulate(chromosomes[atChromosome], new Vector3(0, 0, i*distanceBetweenSimulations), token,assignmentID,this);

                        atChromosome++;
                    }

                }
                

            }*/

                await Task.WhenAll(tasks);
                CloseSimulations(simulations);
                //Debug.Log("RB's = "+GameObject.FindObjectsOfType<Rigidbody>(true).Length);
                await Task.Delay(100);

            }
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
            simulations.Clear();
        }
        private void CloseSimulations(Simulation[] simulations)
        {

            foreach (var simulation in simulations)
            {
                simulation.EndSimulation();
                Destroy(simulation.gameObject);
            }
        }

        private void OnDestroy()
        {
            PoolableBlock.blockPool.Clear();
        }
        /*
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
        }*/
    }


}