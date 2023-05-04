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

        public int GetNumDimensions()
        {
            return chromosome.NumDimensions;
        }

        public int GetDimensionSize(int dimension)
        {
            return chromosome.DimensionSize[dimension];
        }

        public float[] GetGeneArray()
        {
            return chromosome.GeneArray;
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
            return chromosome.GeneArray.Length;
        }
    }
}