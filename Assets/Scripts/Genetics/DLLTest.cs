using System.Collections;
using System.Collections.Generic;
using Adapters;
using UnityEngine;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm;
using GeneticAlgorithm.CrossOverFunctions;
using GeneticAlgorithm.FitnessFunctions;
using GeneticAlgorithm.MutationFunctions;
using GeneticAlgorithm.SelectionFunctions;
using GeneticControllerr = GeneticAlgorithm.Controller.GeneticController;

public class DLLTest : MonoBehaviour
{
    // Start is called before the first frame update
    public GeneticAlgorithm.Controller.GeneticController GeneticAlgorithme;
    private ConsoleController consoleController= new ConsoleController();
    void Start()
    {
        consoleController= new ConsoleController();
        GeneticAlgorithme = new GeneticControllerr(consoleController);
        var cross = new AdapterCrossOverFunction(new SimpleCut(0, 0, 10, consoleController));
        GeneticAlgorithme.AddCrossOverFunction(cross);
        GeneticAlgorithme.AddMutationFunction(new AdapterMutationFunction(new BasicMutation.BasicMutator(0,1,0.1f)));
        GeneticAlgorithme.AddFitnessFunction(new AdapterFitnessFunction(new BasicFitness()));
        GeneticAlgorithme.AddSelectionFunction(new AdapterSelectionFunction(new AddScores(consoleController)));
        GeneticAlgorithme.Start(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
