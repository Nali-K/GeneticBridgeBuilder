using System.Collections.Generic;

namespace GeneticAlgorithm.CrossOverFunctions.Interfaces
{
    public interface IChromosome
    {


        List<IChromosome> Ancestors{  set; }
        IChromosome CreateNewChromosome(int[] dimensions);

        long ID { get; }
        
        int ChozenBy { get; }
        int GetNumDimensions();
        int GetDimensionSize(int dimension);
        void Fill(float[] values);
        void Fill(float min,float max);
        float[] GetGeneArray();
        Dictionary<int[], float> GetValuesAndPositions(int[] position);
        void InsertValues(Dictionary<int[], float> values);

        string ToString();
    }
}