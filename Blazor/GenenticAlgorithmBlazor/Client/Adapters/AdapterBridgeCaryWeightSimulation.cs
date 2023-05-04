using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.FitnessFunctions.Interfaces;
using GenenticAlgorithmBlazor.Shared;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Enums;
using GenenticAlgorithmBlazor.Client.Controllers;
using GenenticAlgorithmBlazor.Client.Interfaces;
using GeneticAlgorithm.FitnessFunctions.Enums;
using GeneticAlgorithm.VisualisationFunctions.Interfaces;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using IChromosome = GeneticAlgorithm.FitnessFunctions.Interfaces.IChromosome;

namespace Adapters
{
    public class AdapterBridgeCaryWeightSimulation:BaseUnitySimulation,IBridgeCaryWeightSimulation,IImageSimulation
    {

        private readonly ISimulationRequest _simulationRequest;
        private List<SharedChromosome> resultChromosomes = new List<SharedChromosome>();
        private string _evolutionWorldName;
        public AdapterBridgeCaryWeightSimulation(ISimulationRequest simulationRequest,string evolutionWorldName)
        {
           _simulationRequest=simulationRequest;
           _evolutionWorldName = evolutionWorldName;
        }
        public async Task<bool> SimulateAsync(List<IChromosome> chromosomes)
        {
            var assignments = GetAssignment(_evolutionWorldName, chromosomes, "dropblock");
            var tasks = new List<Task<SimulationAssigment>>();
            foreach (var assignment in assignments)
            {
                tasks.Add(_simulationRequest.RequestSimulation("AssignmentTest",assignment ));

            }

            var t= await Task.WhenAll(tasks);

            
            
            
            foreach (var result in t)
            {
                resultChromosomes.AddRange(result.chromosomes);
            }


            return false;

        }

        public float GetMaxWeight(IChromosome chromosome)
        {
            foreach (var resultChromosome in resultChromosomes)
            {
                
                if(resultChromosome.ID==chromosome.ID)
                        return resultChromosome.simulationResults["dropblock"];

                    
            }
            Console.Write("can't find chromosome");
            return -1;
        }

        public async Task<bool> SimulateAsync(List<GeneticAlgorithm.VisualisationFunctions.Interfaces.IChromosome> chromosomes)
        {
            return true;
        }

        public Dictionary<string, string> GetImages(GeneticAlgorithm.VisualisationFunctions.Interfaces.IChromosome chromosomes)
        {
            foreach (var resultChromosome in resultChromosomes)
            {
                
                if(resultChromosome.ID==chromosomes.ID)
                        return resultChromosome.visualisationsResults;

                    
            }
            Console.Write("can't find chromosome");
            return null;
        }
    }
}


public class Example
{
    public static void Main()
    {
        var tasks = new Task<long>[10];
        for (int ctr = 1; ctr <= 10; ctr++) {
            int delayInterval = 18 * ctr;
            tasks[ctr - 1] = Task.Run(async () => { long total = 0;
                await Task.Delay(delayInterval);
                var rnd = new Random();
                // Generate 1,000 random numbers.
                for (int n = 1; n <= 1000; n++)
                    total += rnd.Next(0, 1000);

                return total; } );
        }
        var continuation = Task.WhenAll(tasks);
        try {
            continuation.Wait();
        }
        catch (AggregateException)
        {}
   
        if (continuation.Status == TaskStatus.RanToCompletion) {
            long grandTotal = 0;
            foreach (var result in continuation.Result) {
                grandTotal += result;
                Console.WriteLine("Mean: {0:N2}, n = 1,000", result/1000.0);
            }
   
            Console.WriteLine("\nMean of Means: {0:N2}, n = 10,000",
                grandTotal/10000);
        }
        // Display information on faulted tasks.
        else { 
            foreach (var t in tasks)
                Console.WriteLine("Task {0}: {1}", t.Id, t.Status);
        }
    }
}
// The example displays output like the following:
//       Mean: 506.38, n = 1,000
//       Mean: 501.01, n = 1,000
//       Mean: 505.36, n = 1,000
//       Mean: 492.00, n = 1,000
//       Mean: 508.36, n = 1,000
//       Mean: 503.99, n = 1,000
//       Mean: 504.95, n = 1,000
//       Mean: 508.58, n = 1,000
//       Mean: 490.23, n = 1,000
//       Mean: 501.59, n = 1,000
//
//       Mean of Means: 502.00, n = 10,000