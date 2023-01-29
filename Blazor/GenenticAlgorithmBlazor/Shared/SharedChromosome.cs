using System;
using System.Collections.Generic;
using Enums;
using GeneticAlgorithm.FitnessFunctions.Enums;
using GeneticAlgorithm.FitnessFunctions.Interfaces;

namespace GenenticAlgorithmBlazor.Shared
{/// <summary>
 /// contains a chromosome and the results of simulations ran using it
 /// </summary>
    [System.Serializable]
    public class SharedChromosome:IChromosome
    {
        public int numDimentions {get; set; }

        public int[] dimensionSize { get; set; }
        public float[] geneArray { get; set; }
        public Dictionary<string, float> simulationResults { get; set; }

        public int id { get; set; }
        public SharedChromosome()
        {
            simulationResults = new Dictionary<string, float>();
        }
        public SharedChromosome(GeneticAlgorithm.Controller.Chromosome chromosome)
        {
            numDimentions = chromosome.numDimensions;
            geneArray = chromosome.geneArray;
            dimensionSize=chromosome.dimensionSize;
            simulationResults = new Dictionary<string, float>();

            
        }



    }
}