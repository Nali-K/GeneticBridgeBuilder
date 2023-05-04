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
    public class AdapterStabilitySimulation:BaseUnitySimulation,IBridgeStabilitySimulation
    {

        private readonly ISimulationRequest _simulationRequest;
        
        private string _evolutionWorldName;
        public AdapterStabilitySimulation(ISimulationRequest simulationRequest,string evolutionWorldName)
        {
           _simulationRequest=simulationRequest;
           _evolutionWorldName = evolutionWorldName;
        }
        public async Task<bool> SimulateAsync(List<IChromosome> chromosomes)
        {
            var assignments = GetAssignment(_evolutionWorldName, chromosomes, "stability");
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



  
/*
        public Dictionary<string, string> GetImages(GeneticAlgorithm.VisualisationFunctions.Interfaces.IChromosome chromosomes)
        {
            foreach (var resultChromosome in resultChromosomes)
            {
                
                if(resultChromosome.ID==chromosomes.ID)
                        return resultChromosome.visualisationsResults;

                    
            }
            Console.Write("can't find chromosome");
            return null;
        }*/




        public float GetOverallVelocity(IChromosome chromosomes)
        {
            foreach (var resultChromosome in resultChromosomes)
            {
                
                if(resultChromosome.ID==chromosomes.ID)
                    return resultChromosome.simulationResults["stability_velocity"];

                    
            }
            Console.Write("can't find chromosome");
            return -1;

        }

        public float GetDifference(IChromosome chromosomes)
        {
            foreach (var resultChromosome in resultChromosomes)
            {
                
                if(resultChromosome.ID==chromosomes.ID)
                    return resultChromosome.simulationResults["stability_difference"];

                    
            }
            Console.Write("can't find chromosome");
            return -1;

        }
    }
}