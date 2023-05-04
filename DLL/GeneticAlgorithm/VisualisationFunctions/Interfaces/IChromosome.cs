using System.Collections.Generic;

namespace GeneticAlgorithm.VisualisationFunctions.Interfaces
{
    public interface IChromosome
    {
        
        long ID  { get; set; }
        int numDimentions {get; set; }

        int[] dimensionSize { get; set; }
        float[] geneArray { get; set; }
        
        //name, location of image
        Dictionary<string,string> visualisationsResults { get; set; }
    }
}