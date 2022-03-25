using System.Collections.Generic;

namespace MutationFunctions.Interfaces
{
    public interface IChromosome
    {

        void Fill(float[] values);

        float[] GetGeneArray();
        Dictionary<int[], float> GetValuesAndPositions(int[] position);
        void InsertValues(Dictionary<int[], float> values);
        int GetTotalSize();
    }
}