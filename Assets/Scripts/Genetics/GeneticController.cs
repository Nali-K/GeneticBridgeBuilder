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

public class GeneticController : MonoBehaviour
{

    // Start is called before the first frame update
    public float[] dim0;
    public float[] dim1;
    public float[] dim2;
    public float[] dim3;
    private int generation = 0;
    public List<Chromosome> Chromosomes = new List<Chromosome>();
    private Mutator mutator;
    public int numBest =5;
    public int startNum = 25;
    public List<Merger> mergers= new List<Merger>();
    public bool evolve = false;
    public List<FitnessFunction> fitnessFunctions = new List<FitnessFunction>();
    public Chromosome chromosome;
    public Chromosome chromosome2;
    public Chromosome chromosome3;
    private CancellationTokenSource cancellationTokenSource;
    public ColorSim sim;
    void Start()
    {

            DontDestroyOnLoad(this);
            //SceneManager.LoadSceneAsync("SimulationScene");
            mutator = new BasicMutator(-10, 10, 0.05f);
            //int[] dimensions = {100, 100, 100};
            //chromosome = new Chromosome(dimensions);
            int[] dimensions ={10, 10, 10};
            /*chromosome2 = new Chromosome(dimensions);
            chromosome.FillRandom(1, 1);
            chromosome2.FillRandom(2, 2);
            
            var m = new SimpleCut(0, 3);
            chromosome3 = m.Merge(new[] {chromosome, chromosome2});
            sim.Simulate(chromosome3,new Vector3(0,0,0));
            
            m = new SimpleCut(1, 3);
            chromosome3 = m.Merge(new[] {chromosome, chromosome2});

            sim.Simulate(chromosome3,new Vector3(0,15,0));
            
            m=new SimpleCut(2, 3);
            chromosome3 = m.Merge(new[] {chromosome, chromosome2});
            sim.Simulate(chromosome3,new Vector3(0,30,0));
            
            m=new SimpleCut(3, 3);
            chromosome3 = m.Merge(new[] {chromosome, chromosome2});
            sim.Simulate(chromosome3, new Vector3(0, 45, 0));
            
            printdict(chromosome.GetValuesAndPositions(new int[] {-1, -1, -1}));
            printarray(chromosome.GetValues(new int[] {0, -1, 1}));
            printarray(chromosome.GetValues(new int[] {0, -1, 2}));
            printarray(chromosome.GetValues(new int[] {0, -1, 0}));
            printarray(chromosome.GetValues(new int[] {0, -1, 1}));
            printarray(chromosome.GetValues(new int[] {0, -1, 2}));*/
            mergers.Add(new SimpleCut(0,-10,10));
            mergers.Add(new SimpleCut(1,-10,10));
            mergers.Add(new SimpleCut(2,-10,10));

            fitnessFunctions.Add(new BasicFitness());
            for (var i = 0; i < startNum; i++)
            {
                var c = new Chromosome(dimensions);
                c.FillRandom(-10, 10);
                Chromosomes.Add(c);
            }

            cancellationTokenSource = new CancellationTokenSource();
            Evolve(cancellationTokenSource.Token);

    }

    private void OnApplicationQuit()
    {
        cancellationTokenSource.Cancel();
        
    }

    private void printarray(float[]a)
    {
        var s = "";
        foreach (var VARIABLE in a)
        {
            s += VARIABLE;
            s += " ";
        }
            
        Debug.Log(s);
    }

    private void printdict(Dictionary<int[],float> a)
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
        
    }

     private async void Evolve(CancellationToken token)
    {
   

        while (true)
        {
            var d = new Dictionary< Chromosome,float>();
            Debug.Log("----------------- GENERATION: "+generation+" -----------");
            foreach (var c in Chromosomes)
            {
                var s =await fitnessFunctions[0].CalculateFitness(c,token);
            
                d.Add(c,s);
            }
            var sortedDict = from entry in d orderby entry.Value descending select entry ;
            
            
            List<Chromosome> winners = new List<Chromosome>();
            for (var i = 0; i < numBest; i++)
            {
                var c = sortedDict.ElementAt(i);
                Debug.Log(c.Value+" "+c.Key.ToString());
                winners.Add(c.Key);
            }

            Breed(winners);
            generation++;
            //SceneManager.LoadSceneAsync("SampleScene");
            await Task.Delay(1000);
        }
        
    }

    private void Breed(List<Chromosome> l)
    {

        Chromosomes.Clear();
        for (int i = 0; i < numBest; i++)
        {
            Chromosomes.Add(l[i]);
        }

        foreach (var m in mergers)
        {
            executeMerge(m,l);
        }

        for (int i = numBest; i < Chromosomes.Count; i++)
        {
            mutator.Mutate(Chromosomes[i]);
        }

        var s = "";
        /*for (int i = 0; i < 30; i++)
        {
            s += "(";
            s += i-15;
            s += ":";
            s += (mutator as BasicMutator).values[i];
            s += ")";
        }
        Debug.Log(s);*/
    }

    private void executeMerge(Merger m,List<Chromosome> l)
    {
        for (int i = 0; i < numBest; i++)
        {
            for (int j = 0; j < numBest; j++)
            {
                 
                if (i != j)
                {
                    Chromosomes.Add(m.Merge(new[] {l[i],l[j]}));
                }

            }
        }
    }
}
