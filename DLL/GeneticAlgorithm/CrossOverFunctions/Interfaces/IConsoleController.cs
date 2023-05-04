namespace GeneticAlgorithm.CrossOverFunctions.Interfaces
{
    public interface IConsoleController
    {
        void LogMessage(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}