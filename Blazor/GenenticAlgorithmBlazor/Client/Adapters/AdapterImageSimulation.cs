using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller;
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
using IChromosome = GeneticAlgorithm.VisualisationFunctions.Interfaces.IChromosome;

namespace Adapters
{
    public class AdapterImageSimulation:IImageSimulation
    {

        private readonly ISimulationRequest _simulationRequest;
        private List<SharedChromosome> resultChromosomes = new List<SharedChromosome>();
        public AdapterImageSimulation(ISimulationRequest simulationRequest)
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

            assignement.Simulation = "picture";

            TestText testText = new();
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.None;
            
            // Console.WriteLine(JsonConvert.SerializeObject(testlist,settings));
            //Console.WriteLine(JsonConvert.SerializeObject(assignement,settings));
            //var cont = new httpcontent
            //var response = await  http.PostAsJsonAsync("AssignmentTest",assignement);
            testText.testText="test1";
            var r =await _simulationRequest.RequestSimulation("AssignmentTest", assignement);
            resultChromosomes.AddRange(r.chromosomes);
            
           // var response = await http.PostAsJsonAsync("SimpleText", testText);
                // Process the valid form
                return false;
                //throw new System.NotImplementedException();
        }
        public Dictionary<string, string> GetImages(IChromosome chromosomes)
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