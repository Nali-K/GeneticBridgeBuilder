using System.Collections.Generic;

namespace GeneticAlgorithm.FitnessFunctions.Interfaces
{
    public interface IChromosome
    {

        int GetNumDimensions();
        int GetDimensionSize(int dimension);

        float[] GetGeneArray();
        Dictionary<int[], float> GetValuesAndPositions(int[] position);

    }
}