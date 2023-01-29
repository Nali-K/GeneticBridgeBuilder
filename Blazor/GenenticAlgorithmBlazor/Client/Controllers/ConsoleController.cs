
using System;
using System.Collections.Generic;
using controller= GeneticAlgorithm.Controller ;
using fitness= GeneticAlgorithm.FitnessFunctions.Interfaces ;
using crossover=GeneticAlgorithm.CrossOverFunctions.Interfaces;
using mutation = GeneticAlgorithm.MutationFunctions.Interfaces;
using selection =GeneticAlgorithm.SelectionFunctions.Interfaces;
/// <summary>
/// allows the DLL to write to the console
/// </summary>
public class ConsoleController : 
    controller.IConsoleController, 
    fitness.IConsoleController,
    crossover.IConsoleController, 
    mutation.IConsoleController, 
    selection.IConsoleController
{
    public List<string> outputMessages= new List<string>();
    public List<string>  outputWarnings =new List<string>();
    public List<string>  outputErrors = new List<string>();
        void controller.IConsoleController.LogMessage(string message)
        {
            Console.WriteLine(message);

            
        }

        void selection.IConsoleController.LogWarning(string message)
        {
            Console.WriteLine(message);

        }

        void selection.IConsoleController.LogError(string message)
        {
            Console.WriteLine(message);

        }

        void selection.IConsoleController.LogMessage(string message)
        {
            Console.WriteLine(message);

        }

        void mutation.IConsoleController.LogWarning(string message)
        {
            Console.WriteLine(message);

        }

        void mutation.IConsoleController.LogError(string message)
        {
            Console.WriteLine(message);

        }

        void mutation.IConsoleController.LogMessage(string message)
        {
            Console.WriteLine(message);

        }

        void crossover.IConsoleController.LogWarning(string message)
        {
            Console.WriteLine(message);

        }

        void crossover.IConsoleController.LogError(string message)
        {
            Console.WriteLine(message);

        }

        void crossover.IConsoleController.LogMessage(string message)
        {
            Console.WriteLine(message);

        }

        void fitness.IConsoleController.LogWarning(string message)
        {
            Console.WriteLine(message);

        }

        void fitness.IConsoleController.LogError(string message)
        {
            Console.WriteLine(message);

        }

        void fitness.IConsoleController.LogMessage(string message)
        {
            Console.WriteLine(message);

        }

        void controller.IConsoleController.LogWarning(string message)
        {
            Console.WriteLine(message);

        }

        void controller.IConsoleController.LogError(string message)
        {
            Console.WriteLine(message);

        }
    }
