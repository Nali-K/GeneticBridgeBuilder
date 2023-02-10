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
    public class AdapterBridgeCaryWeightSimulation:IBridgeCaryWeightSimulation,IImageSimulation
    {

        private readonly ISimulationRequest _simulationRequest;
        private List<SharedChromosome> resultChromosomes = new List<SharedChromosome>();
        public AdapterBridgeCaryWeightSimulation(ISimulationRequest simulationRequest)
        {
           _simulationRequest=simulationRequest;
        }
        public async Task<bool> SimulateAsync(List<IChromosome> chromosomes)
        {
            var test = _simulationRequest;
            var assignement = new SimulationAssigment();
            var testlist = new List<IChromosome>();
            var testdict = new Dictionary<IChromosome,string>();
            var chromsomeid = 0;
            foreach (var chromosome in chromosomes)
            {

                (chromosome as SharedChromosome).id = chromsomeid;
                chromsomeid++;
                assignement.chromosomes.Add(chromosome as SharedChromosome);
                testlist.Add(chromosome);
                testdict.Add(chromosome,"test");
            }

            assignement.Simulation = "dropblock";

            TestText testText = new();
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.None;
            

            testText.testText="test1";
            var r =await _simulationRequest.RequestSimulation("AssignmentTest", assignement);
            resultChromosomes.AddRange(r.chromosomes); return false;

        }

        public float GetMaxWeight(IChromosome chromosome)
        {
            foreach (var resultChromosome in resultChromosomes)
            {
                
                if(Enumerable.SequenceEqual(chromosome.dimensionSize, resultChromosome.dimensionSize))
                    if(Enumerable.SequenceEqual(chromosome.geneArray, resultChromosome.geneArray))
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
                
                if(Enumerable.SequenceEqual(chromosomes.dimensionSize, resultChromosome.dimensionSize))
                    if(Enumerable.SequenceEqual(chromosomes.geneArray, resultChromosome.geneArray))
                        return resultChromosome.visualisationsResults;

                    
            }
            Console.Write("can't find chromosome");
            return null;
        }
    }
}