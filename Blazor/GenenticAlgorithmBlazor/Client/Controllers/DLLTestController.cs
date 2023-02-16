using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Adapters;
using GenenticAlgorithmBlazor.Client.Controllers;
using GenenticAlgorithmBlazor.Client.Interfaces;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm;
using GeneticAlgorithm.Controller.Models;
using GeneticAlgorithm.CrossOverFunctions;
using GeneticAlgorithm.FitnessFunctions;
using GeneticAlgorithm.MutationFunctions;
using GeneticAlgorithm.SelectionFunctions;
using GeneticAlgorithm.VisualisationFunctions;
using Microsoft.AspNetCore.Components;
using GeneticControllerr = GeneticAlgorithm.Controller.GeneticController;

public class DLLTestController
{

    public GeneticAlgorithm.Controller.GeneticController GeneticAlgorithme;
    private ConsoleController _consoleController;
    private UserInterfaceController _userInterfaceController;
    private ISimulationRequest _simulationRequest;
    public Dictionary<IFitnessFunction, string> fitnessFunctionNames=new Dictionary<IFitnessFunction, string>();
    public EvolutionWorld EvolutionWorld;
    private int targetGenerations=0;
    public DLLTestController(ISimulationRequest simulationRequest)
    {
        _simulationRequest = simulationRequest;
    }

    public void Init(ConsoleController consoleController, UserInterfaceController userInterfaceController)
    {
        var targetBreedingPopulation = 5;
        var chromosomeShape = new[] {1, 6, 6};
        var chromosomeBaseData = new ChromosomeBaseData
        {
            chromosomeShape = chromosomeShape,fillValueMin = 0,fillValueMax = 1,chromosomesUseWholeNumbers = true
        };

        EvolutionWorld = new EvolutionWorld(chromosomeShape,chromosomeBaseData);
        this._consoleController = consoleController;
        this._userInterfaceController = userInterfaceController;
        GeneticAlgorithme = new GeneticController(userInterfaceController, consoleController);
        var cross = new AdapterCrossOverFunction(new SimpleCut(0, 0, 2, consoleController),EvolutionWorld);
        var cross2 = new AdapterCrossOverFunction(new SimpleCut(1, 0, 2, consoleController),EvolutionWorld);
        var cross3 = new AdapterCrossOverFunction(new SimpleCut(2, 0, 2, consoleController),EvolutionWorld);
        GeneticAlgorithme.AddCrossOverFunction(cross);
        GeneticAlgorithme.AddCrossOverFunction(cross2);
        GeneticAlgorithme.AddCrossOverFunction(cross3);
        GeneticAlgorithme.AddMutationFunction(
            new AdapterMutationFunction(new BasicMutation.BasicMutator(0, 1, 0.15f, consoleController)));

        var basicFitnessFuncion = new AdapterFitnessFunction(new BasicFitness());
        fitnessFunctionNames.Add(basicFitnessFuncion,"Cost Fitness Function");
        var bridgeWeightSimulation = new AdapterBridgeCaryWeightSimulation(_simulationRequest);
        var weightFitnessFuncion =
            new AdapterFitnessFunction(
                new BridgeCheckFitness(bridgeWeightSimulation));
        fitnessFunctionNames.Add(weightFitnessFuncion,"Weight Fitness Function");
        GeneticAlgorithme.AddFitnessFunction(basicFitnessFuncion);
        
        GeneticAlgorithme.AddFitnessFunction(weightFitnessFuncion);
        var minRequirements = new Dictionary<string, float> {{"Weight Fitness Function", 0}};
        GeneticAlgorithme.AddSelectionFunction(new AdapterSelectionFunction(new AddScoresMinimumRequirment(consoleController, targetBreedingPopulation,minRequirements),fitnessFunctionNames));
        //GeneticAlgorithme.AddSelectionFunction(new AdapterSelectionFunction(new AddScores(consoleController, 5),fitnessFunctionNames));
        GeneticAlgorithme.VisualisationFunction.Add(new AdapterVisualisationFunction(new BridgeVisualisationImages(new AdapterImageSimulation(_simulationRequest))));
        GeneticAlgorithme.VisualisationFunction.Add(new AdapterVisualisationFunction(new BridgeVisualisationImages(bridgeWeightSimulation)));

        var initialPopulations =Chromosome.InitNewPopulation(targetBreedingPopulation,EvolutionWorld );
        EvolutionWorld.generations.Add(new Generation(initialPopulations));
        
        
       
    }



    public async Task StartAsync()
    {
        Console.Write("start");
        targetGenerations += 500;
        while (EvolutionWorld.NumberFinishedGenerations() <= targetGenerations)
        {
            EvolutionWorld.GetCurrentGeneration().startTime=DateTime.Now;
            var result = await GeneticAlgorithme.EvolveAsync(EvolutionWorld, CancellationToken.None);
            EvolutionWorld.GetCurrentGeneration().completedTime=DateTime.Now;
            EvolutionWorld.GetCurrentGeneration().completed = true;
            _userInterfaceController.DisplayGeneration(EvolutionWorld.NumberFinishedGenerations(),EvolutionWorld.NumberFinishedGenerations()-targetGenerations,EvolutionWorld.GetCurrentGeneration());
            EvolutionWorld.generations.Add(new Generation(result));
        }
        
        Console.Write("started");
    }
}
