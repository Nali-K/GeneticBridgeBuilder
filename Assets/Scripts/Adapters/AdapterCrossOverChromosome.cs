using System.Collections.Generic;
using GeneticAlgorithm.CrossOverFunctions.Interfaces;
using Chromosome = GeneticAlgorithm.Controller.Chromosome;

namespace Adapters
{
    public class AdapterCrossOverChromosome:GeneticAlgorithm.CrossOverFunctions.Interfaces.IChromosome
    {
        public GeneticAlgorithm.Controller.Chromosome chromosome;

        public AdapterCrossOverChromosome(GeneticAlgorithm.Controller.Chromosome chromosome)
        {
            this.chromosome = chromosome;
        }

        public IChromosome CreateNewChromosome(int[] dimensions)
        {
            return new AdapterCrossOverChromosome(new GeneticAlgorithm.Controller.Chromosome(dimensions));
        }

        public int GetNumDimensions()
        {
            return chromosome.numDimensions;
        }

        public int GetDimensionSize(int dimension)
        {
            return chromosome.dimensionSize[dimension];
        }

        public void Fill(float[] values)
        {
            chromosome.Fill(values);
        }

        public void Fill(float min, float max)
        {
            chromosome.FillRandom(min,max);
        }

        public float[] GetGeneArray()
        {
            return chromosome.geneArray;
        }

        public Dictionary<int[], float> GetValuesAndPositions(int[] position)
        {
            return chromosome.GetValuesAndPositions((position));
        }

        public void InsertValues(Dictionary<int[], float> values)
        {
            chromosome.InsertValues(values);
        }

        public string ToString()
        {
            return chromosome.ToString();
        }
    }
}