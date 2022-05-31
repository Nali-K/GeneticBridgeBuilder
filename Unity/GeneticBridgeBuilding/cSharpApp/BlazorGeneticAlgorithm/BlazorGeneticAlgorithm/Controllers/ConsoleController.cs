
using controller= GeneticAlgorithm.Controller ;
using fitness= GeneticAlgorithm.FitnessFunctions.Interfaces ;
using crossover=GeneticAlgorithm.CrossOverFunctions.Interfaces;
using mutation = GeneticAlgorithm.MutationFunctions.Interfaces;
using selection =GeneticAlgorithm.SelectionFunctions.Interfaces;

public class ConsoleController : 
    controller.IConsoleController, 
    fitness.IConsoleController,
    crossover.IConsoleController, 
    mutation.IConsoleController, 
    selection.IConsoleController
    {
        void controller.IConsoleController.LogMessage(string message)
        {
            Console.Write(message);
        }

        void selection.IConsoleController.LogWarning(string message)
        {
            Console.Write(message);
        }

        void selection.IConsoleController.LogError(string message)
        {
            Console.Error.Write(message);
        }

        void selection.IConsoleController.LogMessage(string message)
        {
            Console.Write(message);
        }

        void mutation.IConsoleController.LogWarning(string message)
        {
            Console.Write(message);
        }

        void mutation.IConsoleController.LogError(string message)
        {
            Console.Error.Write(message);
        }

        void mutation.IConsoleController.LogMessage(string message)
        {
            Console.Write(message);
        }

        void crossover.IConsoleController.LogWarning(string message)
        {
            Console.Write(message);
        }

        void crossover.IConsoleController.LogError(string message)
        {
            Console.Error.Write(message);
        }

        void crossover.IConsoleController.LogMessage(string message)
        {
            Console.Write(message);
        }

        void fitness.IConsoleController.LogWarning(string message)
        {
            Console.Write(message);
        }

        void fitness.IConsoleController.LogError(string message)
        {
            Console.Error.Write(message);
        }

        void fitness.IConsoleController.LogMessage(string message)
        {
            Console.Write(message);
        }

        void controller.IConsoleController.LogWarning(string message)
        {
            Console.Write(message);
        }

        void controller.IConsoleController.LogError(string message)
        {
            Console.Error.Write(message);
        }
    }
