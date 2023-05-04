using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Adapters;
using GenenticAlgorithmBlazor.Client.Controllers;
using GenenticAlgorithmBlazor.Client.Interfaces;
using GenenticAlgorithmBlazor.Shared;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm;
using GeneticAlgorithm.Controller.Models;
using GeneticAlgorithm.CrossOverFunctions;
using GeneticAlgorithm.FitnessFunctions;
using GeneticAlgorithm.MutationFunctions;
using GeneticAlgorithm.SelectionFunctions;
using GeneticAlgorithm.SelectionFunctions.Interfaces;
using GeneticAlgorithm.VisualisationFunctions;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using GeneticControllerr = GeneticAlgorithm.Controller.GeneticController;

public class DLLTestController
{

    public GeneticAlgorithm.Controller.GeneticController GeneticAlgorithme;
    private ConsoleController _consoleController;
    private UserInterfaceController _userInterfaceController;
    private ISimulationRequest _simulationRequest;
    private string evolutionWorldName;
    public EvolutionWorld? EvolutionWorld;
    private int targetGenerations=0;
    public DLLTestController(ISimulationRequest simulationRequest)
    {
        _simulationRequest = simulationRequest;

    }

    public async Task<bool>SetEvolutionWorld(int[] chromosomeShape,int targetBreedingPopulation)
    {
        var chromosomeBaseData = new ChromosomeBaseData
        {
            chromosomeShape = chromosomeShape,fillValueMin = 0,fillValueMax = 1,chromosomesUseWholeNumbers = true
        };
        EvolutionWorld = new EvolutionWorld(chromosomeBaseData);
        var initialPopulations =Chromosome.InitNewPopulation(targetBreedingPopulation,EvolutionWorld,_consoleController );
        EvolutionWorld.generations.Add(new Generation(initialPopulations,0));
        targetGenerations = 0;
        return true;
    }

    public async Task<bool> SetNewName(string name)
    {
        using var client = new HttpClient();
        var result = await client.GetAsync(new Uri("https://localhost:7141/AssignmentTest/get_new_save/"+name));
        var nameokay = false;
        var nameCheckOkay=   bool.TryParse(await result.Content.ReadAsStringAsync(),out nameokay);
        if ( nameokay&&nameCheckOkay)
        {
            evolutionWorldName = name;
            return true;
        }
        return false;
    }
    public async Task<bool>SetEvolutionWorld(string name)
    {
        using (var client = new HttpClient())
        {
            var pResult = await client.GetAsync(new Uri("https://localhost:7141/AssignmentTest/get_save_id/"+name));
            var test = await  pResult.Content.ReadAsStringAsync();
            EvolutionWorld = JsonConvert.DeserializeObject<EvolutionWorld>(test);
            var GenerationID = 0;
            var noMoreGenerations = false;
            EvolutionWorld.generations = new List<Generation>();
            do
            {
                var genResult = await client.GetAsync(new Uri("https://localhost:7141/AssignmentTest/get_save_gen_id/" +
                                                              name + "/" + GenerationID.ToString()));
                var genRead = await genResult.Content.ReadAsStringAsync();
                var generation = JsonConvert.DeserializeObject<Generation>(genRead);
                if (generation != null)
                {
                    EvolutionWorld.generations.Add(generation);
                }
                else
                {
                        noMoreGenerations = true;
                }

                GenerationID++;
            } while (!noMoreGenerations);
        }

        targetGenerations = EvolutionWorld.NumberFinishedGenerations();
        evolutionWorldName = name;
        return true;
    }
    public async Task<bool> InitAsync(int targetBreedingPopulation,ConsoleController consoleController, UserInterfaceController userInterfaceController,float mutationPercents,float mutationCopyPercents,float mutationSwapPercents)
    {
        //var targetBreedingPopulation = 5;
        //var chromosomeShape = new[] {1, 6, 6};
        _consoleController = consoleController;
        _userInterfaceController = userInterfaceController;



        GeneticAlgorithme = SetUpGeneticController(EvolutionWorld, _consoleController, _userInterfaceController,
            _simulationRequest, targetBreedingPopulation,mutationPercents/100f,mutationCopyPercents/100f,mutationSwapPercents/100f);




        //var zresult = await );

        return true;
    }


    public int Stop()
    {
        targetGenerations = EvolutionWorld.NumberFinishedGenerations() + 1;
        return targetGenerations;
    }
    public async Task StartAsync(int targetGenerations)
    {
        this.targetGenerations = targetGenerations;
        Console.Write("start");
        while (EvolutionWorld.NumberFinishedGenerations() < this.targetGenerations)
        {
            var t = EvolutionWorld.NumberFinishedGenerations();
            EvolutionWorld.GetCurrentGeneration().startTime=DateTime.Now;
            var result = await GeneticAlgorithme.EvolveAsync(EvolutionWorld, CancellationToken.None);
            EvolutionWorld.GetCurrentGeneration().completedTime=DateTime.Now;
            EvolutionWorld.GetCurrentGeneration().completed = true;
            _userInterfaceController.DisplayGeneration(EvolutionWorld.NumberFinishedGenerations(),EvolutionWorld.NumberFinishedGenerations()-targetGenerations,EvolutionWorld.GetCurrentGeneration());


            var GenerationList = EvolutionWorld.generations;
            EvolutionWorld.generations = null;
            var evolutionWorldWrapper = new EvolutionWorldWrapper
            {
                name = evolutionWorldName,
                json = JsonConvert.SerializeObject(EvolutionWorld)
            };
             EvolutionWorld.generations=GenerationList;
             var generationWrapper = new GenerationWrapper()
             {
                 EvolutionWorld = evolutionWorldName,
                 ID=EvolutionWorld.GetCurrentGeneration().ID,
                 json = JsonConvert.SerializeObject(EvolutionWorld.GetCurrentGeneration())
             };
             EvolutionWorld.generations.Add(new Generation(result,EvolutionWorld.NumberFinishedGenerations()));
             var generationWrapperNew = new GenerationWrapper()
             {
                 EvolutionWorld = evolutionWorldName,
                 ID=EvolutionWorld.GetCurrentGeneration().ID,
                 json = JsonConvert.SerializeObject(EvolutionWorld.GetCurrentGeneration())
             };
            using (var client = new HttpClient())
            {
            //var zresult = await );

            var saveJson =JsonConvert.SerializeObject(evolutionWorldWrapper);
            var saveJsonGen =JsonConvert.SerializeObject(generationWrapper);
            var saveJsonGenNew =JsonConvert.SerializeObject(generationWrapperNew);
            var payload = new StringContent(saveJson, Encoding.UTF8, "application/json");
            var payloadGen = new StringContent(saveJsonGen, Encoding.UTF8, "application/json");
            var payloadGenNew = new StringContent(saveJsonGenNew, Encoding.UTF8, "application/json");

                //Console.WriteLine(wrapped.json);
            var pResult = await client.PostAsync(new Uri("https://localhost:7141/AssignmentTest/submit_save"),payload);
            var test = await  pResult.Content.ReadAsStringAsync();
            var pResultGen = await client.PostAsync(new Uri("https://localhost:7141/AssignmentTest/submit_save_generation"),payloadGen);
            var testGen = await  pResultGen.Content.ReadAsStringAsync();
            var pResultGenNew = await client.PostAsync(new Uri("https://localhost:7141/AssignmentTest/submit_save_generation"),payloadGenNew);
            var testGenNew = await  pResultGenNew.Content.ReadAsStringAsync();
            }
        }
        
        Console.Write("done");
    }

    private GeneticController SetUpGeneticController(EvolutionWorld evolutionWorld,ConsoleController consoleController, UserInterfaceController userInterfaceController, ISimulationRequest simulationRequest,int targetBreedingPopulation, float mutationRatio, float mutationRatioCopy, float mutationRatioSwap)
    {
        var cross = new AdapterCrossOverFunction(new RandomRiffle(0, 0, 2, consoleController,true,true),evolutionWorld,consoleController);
        var cross2 = new AdapterCrossOverFunction(new RandomRiffle(1, 0, 2, consoleController,true,true),evolutionWorld,consoleController);
        var cross3 = new AdapterCrossOverFunction(new RandomRiffle(2, 0, 2, consoleController,true,true),evolutionWorld,consoleController);
        var mutation = new AdapterMutationFunction(new BasicMutator(0, 1, mutationRatio, consoleController));
        var mutation2 = new AdapterMutationFunction(new CopyRowMutation(mutationRatioCopy, consoleController));
        var mutation3 = new AdapterMutationFunction(new SwapRowMutation( mutationRatioSwap, consoleController));
        var basicFitnessFunction = new AdapterFitnessFunction(new BasicFitness(),"Cost Fitness Function");
        //var bridgeWeightSimulation = new AdapterBridgeCaryWeightSimulation(simulationRequest,evolutionWorldName);
        var bridgeRoadSimulation = new AdapterRoadSimulation(simulationRequest,evolutionWorldName);
        var bridgeStabilitySimulation = new AdapterStabilitySimulation(simulationRequest,evolutionWorldName);
        var weightFitnessFunction =
            new AdapterFitnessFunction(
                new BridgeCheckFitness(bridgeRoadSimulation),"Weight Fitness Function");
        var numRoadsFitnessFunction =
            new AdapterFitnessFunction(
                new RoadWidthFitness(bridgeRoadSimulation),"number of roads Fitness Function");
        var flatnessFitnessFunction =
            new AdapterFitnessFunction(
                new RoadLevel(bridgeRoadSimulation),"flatness Fitness Function");
        var stabilityVelocityFitnessFunction =
            new AdapterFitnessFunction(
                new BridgeStabilityDifferenceFitness(bridgeStabilitySimulation),"Stability Difference Fitness Function");
        var stabilityDifferenceFunction =
            new AdapterFitnessFunction(
                new BridgeStabilityVelocityFitness(bridgeStabilitySimulation),"stability Velocity Fitness Function");

        var fitnessFunctionData = new List<IFitnessFunctionData>();
        fitnessFunctionData.Add(new FitnessFunctionData(basicFitnessFunction,targetBreedingPopulation/2,targetBreedingPopulation/6,removeInferiorChildren:false));
        fitnessFunctionData.Add(new FitnessFunctionData(weightFitnessFunction,targetBreedingPopulation/2,targetBreedingPopulation/6,removeInferiorChildren:true));
        fitnessFunctionData.Add(new FitnessFunctionData(numRoadsFitnessFunction,targetBreedingPopulation/2,targetBreedingPopulation/6,removeInferiorChildren:true));
        fitnessFunctionData.Add(new FitnessFunctionData(flatnessFitnessFunction,targetBreedingPopulation/2,targetBreedingPopulation/6,removeInferiorChildren:true));
        fitnessFunctionData.Add(new FitnessFunctionData(stabilityVelocityFitnessFunction,targetBreedingPopulation/2,targetBreedingPopulation/6,removeInferiorChildren:true));
        fitnessFunctionData.Add(new FitnessFunctionData(stabilityDifferenceFunction,targetBreedingPopulation/2,targetBreedingPopulation/6,removeInferiorChildren:true));
        var minRequirements = new Dictionary<string, float> {{"Weight Fitness Function", -100}};

        var selectionAdd = new AdapterSelectionFunction(
            new AddScores(consoleController, targetBreedingPopulation),evolutionWorld);
        var selectionVote = new AdapterSelectionFunction(
            new VotingSelectionFunction(consoleController, fitnessFunctionData),evolutionWorld);
        var selectionAddNormalised = new AdapterSelectionFunction(
            new AddScoresNormalised(consoleController, targetBreedingPopulation),evolutionWorld);
        var visualisationFunction =
            new AdapterVisualisationFunction(new BridgeVisualisationImages(bridgeRoadSimulation),"Bridge Weight");
        

        
        
        var geneticAlgorithme = new GeneticController(userInterfaceController, consoleController);
        geneticAlgorithme.AddCrossOverFunction(cross);
        geneticAlgorithme.AddCrossOverFunction(cross2);
        geneticAlgorithme.AddCrossOverFunction(cross3);
        geneticAlgorithme.AddMutationFunction(mutation);
        geneticAlgorithme.AddFitnessFunction(basicFitnessFunction);
        geneticAlgorithme.AddFitnessFunction(weightFitnessFunction);
        geneticAlgorithme.AddFitnessFunction(numRoadsFitnessFunction);
        geneticAlgorithme.AddFitnessFunction(flatnessFitnessFunction);
        geneticAlgorithme.AddFitnessFunction(stabilityVelocityFitnessFunction);
        geneticAlgorithme.AddFitnessFunction(stabilityDifferenceFunction);
        geneticAlgorithme.AddSelectionFunction(selectionVote);
        geneticAlgorithme.AddVisualisationFunction(visualisationFunction);
        geneticAlgorithme.KeepWinners = true;
        return geneticAlgorithme;

    }


}
