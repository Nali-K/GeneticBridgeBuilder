using System.Collections.Generic;
using GeneticAlgorithm.FitnessFunctions.Enums;

namespace GeneticAlgorithm.FitnessFunctions.Interfaces
{
    public interface IChromosome
    {

        int numDimentions {get; set; }

        int[] dimensionSize { get; set; }
        float[] geneArray { get; set; }

        Dictionary<string,float> simulationResults { get; set; }
    }
}