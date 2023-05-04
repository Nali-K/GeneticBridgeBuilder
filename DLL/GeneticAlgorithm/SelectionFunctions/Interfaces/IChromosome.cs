using System.Collections.Generic;
using System.Dynamic;

namespace GeneticAlgorithm.SelectionFunctions.Interfaces
{
    public interface IChromosome
    {

        List<float> GetScores();
        Dictionary<string,float> GetScoresAndFitnessFunctionNames();

        List<string> ChozenBy { get; set; }


        List<IChromosome> parents { get; }
    }
}