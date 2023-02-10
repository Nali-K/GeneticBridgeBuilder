using System.Collections.Generic;
using GeneticAlgorithm.Controller.Models;

namespace Adapters
{
    public class AdapterMutationChromosome : GeneticAlgorithm.MutationFunctions.Interfaces.IChromosome

    {
        public Chromosome chromosome;

        public AdapterMutationChromosome(Chromosome chromosome)
        {
            this.chromosome = chromosome;
        }
        public void Fill(float[] values)
        {
            chromosome.Fill(values);
        }

        public float[] GetGeneArray()
        {
            return chromosome.geneArray;
        }

        public Dictionary<int[], float> GetValuesAndPositions(int[] position)
        {
            return chromosome.GetValuesAndPositions(position);
        }

        public void InsertValues(Dictionary<int[], float> values)
        {
            chromosome.InsertValues(values);
        }

        public int GetTotalSize()
        {
            return chromosome.geneArray.Length;
        }
    }
}