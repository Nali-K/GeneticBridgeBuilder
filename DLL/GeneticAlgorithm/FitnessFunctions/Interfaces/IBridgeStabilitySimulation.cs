namespace GeneticAlgorithm.FitnessFunctions.Interfaces
{
    public interface IBridgeStabilitySimulation:ISimulation
    {
        float GetOverallVelocity(IChromosome chromosomes);
        float GetDifference(IChromosome chromosomes);
    }
}