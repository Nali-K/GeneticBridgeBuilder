using System.Collections.Generic;

namespace CrossOverFunctions.Interfaces
{
    public interface IChromosome
    {
        IChromosome CreateNewChromosome(int[] dimensions);
        int GetNumDimensions();
        int GetDimensionSize(int dimension);
        void Fill(float[] values);
        void Fill(float min,float max);
        float[] GetGeneArray();
        Dictionary<int[], float> GetValuesAndPositions(int[] position);
        void InsertValues(Dictionary<int[], float> values);
    }
}