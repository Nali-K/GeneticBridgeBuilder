using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.FitnessFunctions.Interfaces;

namespace Adapters
{
    public class AdapterFitnessChromosome : GeneticAlgorithm.FitnessFunctions.Interfaces.IChromosome
    {
        private GeneticAlgorithm.Controller.Chromosome chromosome;

        public AdapterFitnessChromosome(GeneticAlgorithm.Controller.Chromosome chromosome)
        {
            this.chromosome = chromosome;
        }

        public int GetNumDimensions()
        {
            return chromosome.numDimensions;
        }

        public int GetDimensionSize(int dimension)
        {
            return chromosome.dimensionSize[dimension];
        }

        public GeneticAlgorithm.Controller.Chromosome GetChromosome()
        {
            return chromosome;
        }
        

        public float[] GetGeneArray()
        {
            return chromosome.geneArray;
        }

        public Dictionary<int[], float> GetValuesAndPositions(int[] position)
        {
            return chromosome.GetValuesAndPositions(position);
        }


    }
}