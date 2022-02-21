using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticController : MonoBehaviour
{
    public Chromosome TestChromosome1;
    public Chromosome TestChromosome2;
    public Chromosome TestChromosome3;
    public Chromosome TestChromosome4;
    // Start is called before the first frame update
    public float[] dim0;
    public float[] dim1;
    public float[] dim2;
    public float[] dim3;
    void Start()
    {

        int[] dimensions = {4,2,5};
        TestChromosome1 = new Chromosome(dimensions);
        int[] dimensions2 = {9,2,3};
        TestChromosome2 = new Chromosome(dimensions2);
        TestChromosome1.FillRandom(1,9);
        TestChromosome2.FillRandom(-9,-1);
        TestChromosome3 = Chromosome.Merge(TestChromosome1, TestChromosome2);
        TestChromosome4 = Chromosome.Merge(TestChromosome2, TestChromosome1);
        Debug.Log(TestChromosome1.ToString());
        Debug.Log(TestChromosome2.ToString());
        Debug.Log(TestChromosome3.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
