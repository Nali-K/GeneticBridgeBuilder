using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Genetics;
using Genetics.Mergers;
using Genetics.Mutators;
using Simulation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GeneticController : MonoBehaviour
{


    private int generation = 0;
    public List<Chromosome> Chromosomes = new List<Chromosome>();
    private Mutator mutator;
    public int numBest = 5;
    public int startNum = 6;
    public List<Merger> mergers = new List<Merger>();
    public bool evolve = false;
    [SerializeField] public List<FitnessFunction> fitnessFunctions = new List<FitnessFunction>();
    private CancellationTokenSource cancellationTokenSource;
    public ColorSim sim;
    public BridgeCheckFitness lol;
    public GameObject bridgeSimGameObject;
    public int simulatainousChecks = 5;
    public float calcSpeed=0;
    public float oldCalcSpeed=0;
    public float calcSpeedCountDown=0;
    private bool evaluating;
    void Start()
    {

        DontDestroyOnLoad(this);
        mutator = new BasicMutator(0, 1, 0.05f);
        int[] dimensions = {8, 8, 8};

        mergers.Add(new SimpleCut(0, 0, 1));
        mergers.Add(new SimpleCut(1, 0, 1));
        mergers.Add(new SimpleCut(2, 0, 1));
        fitnessFunctions.Add(new BridgeCheckFitness(bridgeSimGameObject));
        fitnessFunctions.Add(new StabilityFitness(bridgeSimGameObject));
        lol = fitnessFunctions[0] as BridgeCheckFitness;
        fitnessFunctions.Add(new BasicFitness());
        for (var i = 0; i < startNum; i++)
        {
            var c = new Chromosome(dimensions);
            c.FillRandom(0, 2);
            Chromosomes.Add(c);
        }

        cancellationTokenSource = new CancellationTokenSource();
        Evolve(cancellationTokenSource.Token);

    }

    private void OnApplicationQuit()
    {
        cancellationTokenSource.Cancel();

    }

    private void printarray(float[] a)
    {
        var s = "";
        foreach (var VARIABLE in a)
        {
            s += VARIABLE;
            s += " ";
        }

        Debug.Log(s);
    }

    private void printdict(Dictionary<int[], float> a)
    {
        var s = "";
        foreach (var VARIABLE in a)
        {
            s += "[";
            foreach (var keyint in VARIABLE.Key)
            {
                s += keyint;
                s += ",";
            }

            s += "] ";
            s += VARIABLE.Value;
            s += ",";
        }

        Debug.Log(s);
    }

    // Update is called once per frame
    void Update()
    {
        getMaxSims();




    }

    private void getMaxSims()
    {
        if (!evaluating) return;
        if (calcSpeedCountDown<=0)
        {
            calcSpeedCountDown = 3;
            if (calcSpeed != 0)
            {
                //calcSpeed;
                if (calcSpeed > 0.08f)
                {
                    simulatainousChecks -= 1;
                }
                if (calcSpeed < 0.05f)
                {
                    simulatainousChecks += 1;
                }
                oldCalcSpeed = calcSpeed;

                calcSpeed = 0;
            }
        }
        else
        {
            calcSpeedCountDown -= Time.fixedDeltaTime;
            if (Time.deltaTime > calcSpeed)
            {
                calcSpeed = Time.deltaTime;
            }
        }

        //Debug.Log(Time.deltaTimef);
    }

    private async void Evolve(CancellationToken token)
    {
        Chromosomes.Clear();
        foreach (var str in System.IO.File.ReadLines(Application.persistentDataPath + "/data.json"))
        {
            Chromosomes.Add(JsonUtility.FromJson<Chromosome>(str));
        }

        while (true)
        {
            var d = new Dictionary<Chromosome, float>();
            Debug.Log("----------------- GENERATION: " + generation + " -----------");

            var sortedDict = from entry in d orderby entry.Value descending select entry;

            await Evaluate(d, token);
            List<Chromosome> winners = new List<Chromosome>();
            for (var i = 0; i < numBest; i++)
            {
                var c = sortedDict.ElementAt(i);
                Debug.Log(c.Value + " " + c.Key.ToString());
                winners.Add(c.Key);
            }

            await Breed(winners, token);
            generation++;
            //SceneManager.LoadSceneAsync("SampleScene");
            await Task.Delay(1000);
            string data = "";
            for (var i = 0; i < sortedDict.Count(); i++)
            {
                //data+="{score:"JsonUtility.ToJson(i);
                //data+="{\"score:\""+sortedDict.ElementAt(i).Value+"}";
                data += JsonUtility.ToJson(sortedDict.ElementAt(i).Key);
                data += "\n";
            }


            System.IO.File.WriteAllText(Application.persistentDataPath + "/data.json", data);
        }

    }

    private async Task Evaluate(Dictionary<Chromosome, float> d, CancellationToken token)
    {
        

        
        var currentlyChecking = new Task<KeyValuePair<Chromosome,float>>[150];
        var checkedChromosomes=0;
        var amountChecking = 0;
        var atChromosome = 0;

        while (checkedChromosomes < Chromosomes.Count())
        {

            for  (var i =0;i<currentlyChecking.Length;i++)
            {
                if (currentlyChecking[i]==null) continue;
                if (!currentlyChecking[i].IsCompleted) continue;
                var restult = currentlyChecking[i].Result;
                d.Add(restult.Key,restult.Value);
                currentlyChecking[i].Dispose();
                currentlyChecking[i] = null;
                checkedChromosomes++;
                amountChecking--;
            }
            if(amountChecking < simulatainousChecks)
            {
                evaluating = false;
                if (atChromosome < Chromosomes.Count)
                {
                    for (var i = 0; i < currentlyChecking.Length; i++)
                    {
                        if (currentlyChecking[i] == null)
                        {
                            var r = RunFitnessChecks(Chromosomes[atChromosome], token,i*30);
                            currentlyChecking[i]=(r);
                            atChromosome++;
                            amountChecking++;
                            break;
                        }

                    }

                }
                
            }
            else
            {
                evaluating = true; 
            }
            await Task.Delay(200, token);
            
        }
        evaluating = false;
    }



    public async Task<KeyValuePair<Chromosome, float>> RunFitnessChecks(Chromosome chromosome, CancellationToken token, float pos)
    {
        var score = 0f;
        var bussy =false;
        foreach (var fitness in fitnessFunctions)
        {
            var cal=fitness.CalculateFitness(chromosome, token,new Vector3(0,0,pos));
            bussy = true;
            var s = 0f;
            while (!cal.IsCompleted)
            {
                await Task.Delay(100, token);
            }

            score += cal.Result;


        }

        return new KeyValuePair<Chromosome, float>(chromosome,score);
    }



private async Task Breed(List<Chromosome> l,CancellationToken token)
    {

        Chromosomes.Clear();
        for (int i = 0; i < numBest; i++)
        {
            Chromosomes.Add(l[i]);
        }

        foreach (var m in mergers)
        {
            executeMerge(m,l);
            await Task.Delay(50,token);
        }

        for (int i = numBest; i < Chromosomes.Count; i++)
        {
            mutator.Mutate(Chromosomes[i]);
            await Task.Delay(50,token);
        }

        var s = "";

    }

    private void executeMerge(Merger m,List<Chromosome> l)
    {
        for (int i = 0; i < numBest; i++)
        {
            for (int j = 0; j < numBest; j++)
            {
                 
                //if (i != j)
                {
                    Chromosomes.Add(m.Merge(new[] {l[i],l[j]}));
                }

            }
        }
    }

    public void Exit()
    {
        cancellationTokenSource.Cancel();
    }
}
