namespace GeneticAlgorithm.FitnessFunctions.Interfaces
{
    public interface IBridgeCaryWeightSimulation:ISimulation
    {
        float GetMaxWeight(IChromosome chromosomes);
    }
}