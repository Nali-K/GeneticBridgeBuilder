namespace FitnessFunctions.Controller
{
    public interface IConsoleController
    {
        void LogMessage(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}