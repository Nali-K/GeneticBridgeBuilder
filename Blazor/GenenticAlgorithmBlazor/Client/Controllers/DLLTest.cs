using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Adapters;
using GenenticAlgorithmBlazor.Client.Controllers;

using GeneticAlgorithm.Controller;
using GeneticAlgorithm;
using GeneticAlgorithm.CrossOverFunctions;
using GeneticAlgorithm.FitnessFunctions;
using GeneticAlgorithm.MutationFunctions;
using GeneticAlgorithm.SelectionFunctions;
using GeneticControllerr = GeneticAlgorithm.Controller.GeneticController;

public class DLLTestController
{

    public GeneticAlgorithm.Controller.GeneticController GeneticAlgorithme;
    private ConsoleController _consoleController;
    private UserInterfaceController _userInterfaceController;
    public DLLTestController()
    {
    }

    public void Init(ConsoleController consoleController, UserInterfaceController userInterfaceController)
    {
        this._consoleController = consoleController;
        this._userInterfaceController = userInterfaceController;
        GeneticAlgorithme = new GeneticControllerr(userInterfaceController, consoleController);
        var cross = new AdapterCrossOverFunction(new SimpleCut(0, 0, 9, consoleController));
        var cross2 = new AdapterCrossOverFunction(new SimpleCut(1, 0, 9, consoleController));
        var cross3 = new AdapterCrossOverFunction(new SimpleCut(2, 0, 9, consoleController));
        GeneticAlgorithme.AddCrossOverFunction(cross);
        GeneticAlgorithme.AddCrossOverFunction(cross2);
        GeneticAlgorithme.AddCrossOverFunction(cross3);
        GeneticAlgorithme.AddMutationFunction(
            new AdapterMutationFunction(new BasicMutation.BasicMutator(0, 9, 0.01f, consoleController)));
        GeneticAlgorithme.AddFitnessFunction(new AdapterFitnessFunction(new BasicFitness()));
        GeneticAlgorithme.AddSelectionFunction(new AdapterSelectionFunction(new AddScores(consoleController, 20)));
        GeneticAlgorithme.InitNewPopulation(20, new[] {8, 8, 8}, 0, 9);
    }



    public async Task StartAsync()
    {
        Console.Write("start");
        await GeneticAlgorithme.StartAsync(50);
        Console.Write("started");
    }
}
