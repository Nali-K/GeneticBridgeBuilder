using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Genetics;
using Genetics.Mergers;
using Genetics.Mutators;
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


    void Start()
    {

            DontDestroyOnLoad(this);
            //SceneManager.LoadSceneAsync("SimulationScene");
            mutator = new BasicMutator(-10, 10, 0.01f);
            int[] dimensions = {1000, 1000, 1000};

            mergers.Add(new SimpleCut());
            fitnessFunctions.Add(new BasicFitness());
            for (var i = 0; i < startNum; i++)
            {
                var c = new Chromosome(dimensions);
                c.FillRandom(-10, 10);
                Chromosomes.Add(c);
            }


            Evolve();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private async void Evolve()
    {
   

        while (true)
        {
            var d = new Dictionary< Chromosome,float>();
            Debug.Log("----------------- GENERATION: "+generation+" -----------");
            foreach (var c in Chromosomes)
            {
                var s =await fitnessFunctions[0].CalculateFitness(c);
            
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
