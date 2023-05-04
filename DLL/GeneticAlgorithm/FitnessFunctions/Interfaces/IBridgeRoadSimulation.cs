namespace GeneticAlgorithm.FitnessFunctions.Interfaces
{
    public interface IBridgeRoadSimulation:ISimulation
    {
        float GetNumRoads(IChromosome chromosomes);
        float GetFlatness(IChromosome chromosomes);
    }
}