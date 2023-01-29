using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Adapters;
using GenenticAlgorithmBlazor.Client.Controllers;
using GenenticAlgorithmBlazor.Client.Interfaces;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm;
using GeneticAlgorithm.CrossOverFunctions;
using GeneticAlgorithm.FitnessFunctions;
using GeneticAlgorithm.MutationFunctions;
using GeneticAlgorithm.SelectionFunctions;
using Microsoft.AspNetCore.Components;
using GeneticControllerr = GeneticAlgorithm.Controller.GeneticController;

public class DLLTestController
{

    public GeneticAlgorithm.Controller.GeneticController GeneticAlgorithme;
    private ConsoleController _consoleController;
    private UserInterfaceController _userInterfaceController;
    private ISimulationRequest _simulationRequest;
    public DLLTestController(ISimulationRequest simulationRequest)
    {
        _simulationRequest = simulationRequest;
    }

    public void Init(ConsoleController consoleController, UserInterfaceController userInterfaceController)
    {
        this._consoleController = consoleController;
        this._userInterfaceController = userInterfaceController;
        GeneticAlgorithme = new GeneticController(userInterfaceController, consoleController);
        var cross = new AdapterCrossOverFunction(new SimpleCut(0, 0, 2, consoleController));
        var cross2 = new AdapterCrossOverFunction(new SimpleCut(1, 0, 2, consoleController));
        var cross3 = new AdapterCrossOverFunction(new SimpleCut(2, 0, 2, consoleController));
        GeneticAlgorithme.AddCrossOverFunction(cross);
        GeneticAlgorithme.AddCrossOverFunction(cross2);
        GeneticAlgorithme.AddCrossOverFunction(cross3);
        GeneticAlgorithme.AddMutationFunction(
            new AdapterMutationFunction(new BasicMutation.BasicMutator(0, 1, 0.01f, consoleController)));
        GeneticAlgorithme.AddFitnessFunction(new AdapterFitnessFunction(new BasicFitness()));
        GeneticAlgorithme.AddFitnessFunction(new AdapterFitnessFunction(new BridgeCheckFitness(new AdapterBridgeCaryWeightSimulation(_simulationRequest))));
        GeneticAlgorithme.AddSelectionFunction(new AdapterSelectionFunction(new AddScores(consoleController, 5)));
        GeneticAlgorithme.InitNewPopulation(5, new[] {3, 5, 5}, 0, 2);
    }



    public async Task StartAsync()
    {
        Console.Write("start");
        await GeneticAlgorithme.StartAsync(10);
        Console.Write("started");
    }
}
