using System.Collections.Generic;

namespace GeneticAlgorithm.MutationFunctions.Interfaces
{
    public interface IChromosome
    {

        void Fill(float[] values);
        int GetNumDimensions();
        int GetDimensionSize(int dimension);
        float[] GetGeneArray();
        Dictionary<int[], float> GetValuesAndPositions(int[] position);
        void InsertValues(Dictionary<int[], float> values);
        int GetTotalSize();
    }
}