using System.Collections.Generic;

namespace GeneticAlgorithm.SelectionFunctions.Interfaces
{
    public interface IChromosome
    {

        List<float> GetScores();
        Dictionary<string,float> GetScoresAndFitnessFunctionNames();
    }
}