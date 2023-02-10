using System.Collections.Generic;

namespace GeneticAlgorithm.VisualisationFunctions.Interfaces
{
    public interface IChromosome
    {

        int numDimentions {get; set; }

        int[] dimensionSize { get; set; }
        float[] geneArray { get; set; }
        
        //name, location of image
        Dictionary<string,string> visualisationsResults { get; set; }
    }
}