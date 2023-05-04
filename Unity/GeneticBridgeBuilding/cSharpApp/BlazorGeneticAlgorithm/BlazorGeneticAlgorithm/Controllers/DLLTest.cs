using System.Collections;
using System.Collections.Generic;
using Adapters;
using BlazorGeneticAlgorithm.Controllers;
using Microsoft.AspNetCore.Mvc;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm;
using GeneticAlgorithm.CrossOverFunctions;
using GeneticAlgorithm.FitnessFunctions;
using GeneticAlgorithm.MutationFunctions;
using GeneticAlgorithm.SelectionFunctions;
using GeneticControllerr = GeneticAlgorithm.Controller.GeneticController;

public class DLLTestController:Controller
{

    public GeneticAlgorithm.Controller.GeneticController GeneticAlgorithme;
    private ConsoleController consoleController= new ConsoleController();
    public DLLTestController(UserInterfaceController userInterfaceController)
    {
        consoleController= new ConsoleController();
        GeneticAlgorithme = new GeneticControllerr(userInterfaceController,consoleController);
        var cross = new AdapterCrossOverFunction(new SimpleCut(0, 0, 9, consoleController));
        var cross2 = new AdapterCrossOverFunction(new SimpleCut(1, 0, 9, consoleController));
        var cross3 = new AdapterCrossOverFunction(new SimpleCut(2, 0, 9, consoleController));
        GeneticAlgorithme.AddCrossOverFunction(cross);
        GeneticAlgorithme.AddCrossOverFunction(cross2);
        GeneticAlgorithme.AddCrossOverFunction(cross3);
        GeneticAlgorithme.AddMutationFunction(new AdapterMutationFunction(new BasicMutation.BasicMutator(0,9,0.01f,consoleController)));
        GeneticAlgorithme.AddFitnessFunction(new AdapterFitnessFunction(new BasicFitness()));
        GeneticAlgorithme.AddSelectionFunction(new AdapterSelectionFunction(new AddScores(consoleController,50)));
        GeneticAlgorithme.InitNewPopulation(10,new []{8,8,8},0,9);

    }


    public void Start()
    {
        Console.Write("start");
        GeneticAlgorithme.Start(10);
        Console.Write("started")
            ;
    }
}
