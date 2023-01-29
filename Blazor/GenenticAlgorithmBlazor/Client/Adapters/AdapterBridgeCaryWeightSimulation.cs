using System;
using System.Collections.Generic;
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
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Adapters
{
    public class AdapterBridgeCaryWeightSimulation:IBridgeCaryWeightSimulation
    {

        private readonly ISimulationRequest _simulationRequest;
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
                /*if (chromosome.GetNumDimensions() != 3)
                {
                    throw new System.Exception();
                }

                var values = new int[chromosome.GetDimensionSize(0), chromosome.GetDimensionSize(1),
                    chromosome.GetDimensionSize(2)];
                var genes = chromosome.GetGeneArray();
                for (var i=0; i<genes.Length;i++)
                {
                    values[i % chromosome.GetDimensionSize(0),
                        (i / chromosome.GetDimensionSize(0))% chromosome.GetDimensionSize(1), 
                        i / (chromosome.GetDimensionSize(0) * chromosome.GetDimensionSize(1))] = (int)genes[i];
                }*/
                //Console.WriteLine(JsonConvert.SerializeObject(chromosome));
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
            
           // Console.WriteLine(JsonConvert.SerializeObject(testlist,settings));
            Console.WriteLine(JsonConvert.SerializeObject(assignement,settings));
            //var cont = new httpcontent
           //var response = await  http.PostAsJsonAsync("AssignmentTest",assignement);
            testText.testText="test1";
            var r =await _simulationRequest.RequestSimulation("AssignmentTest", assignement);
            
           // var response = await http.PostAsJsonAsync("SimpleText", testText);
                // Process the valid form
                return false;
                //throw new System.NotImplementedException();
        }

        public float GetMaxWeight(IChromosome chromosome)
        {
            return 1f;
        }
    }
}