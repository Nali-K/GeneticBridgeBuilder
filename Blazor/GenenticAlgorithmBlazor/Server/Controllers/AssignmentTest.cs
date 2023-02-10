using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GenenticAlgorithmBlazor.Server.Models;
using GenenticAlgorithmBlazor.Shared;
using GeneticAlgorithm.FitnessFunctions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GenenticAlgorithmBlazor.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssignmentTest:ControllerBase
    {
        private readonly ILogger<AssignmentTest> _logger;

        public AssignmentTest(ILogger<AssignmentTest> logger)
        {
            _logger = logger;
        }

        //public void Write(string text)
        //{
        //    System.IO.File.WriteAllText("Data/test.txt", text);
        //}
        [HttpGet("get_by_id/{id:int}")]
        public IActionResult Get(int id)
        {
            //var id = 1;
           //return System.IO.File.ReadAllText("Data/test.txt");
            if (System.IO.File.Exists("Data/assignment_" + id + ".json"))
            {
                var json = System.IO.File.ReadAllText("Data/assignment_" + id + ".json");
                
                var assigment = JsonConvert.DeserializeObject<SimulationAssigment>(json);
                if (assigment == null)
                {
                    return new OkObjectResult(false);
                }
                if (assigment.assignmentStatus.completedSimulation)
                {
                    return new OkObjectResult(json);
                }
                
            }
            return new OkObjectResult(false);
        }
        [HttpGet("get_free")]
        public IActionResult Get()
        {

            //return System.IO.File.ReadAllText("Data/test.txt");
            foreach (var fileName in System.IO.Directory.EnumerateFiles("Data/"))
            {
                if (Regex.IsMatch(fileName, "(assignment_[0-9]+.json)"))
                {
                    var json = System.IO.File.ReadAllText(fileName);
                    var assigment = JsonConvert.DeserializeObject<SimulationAssigment>(json);
                    if (assigment == null)
                    {
                        return new OkObjectResult(false);
                    }
                    if (!assigment.assignmentStatus.startedSimulation&&!assigment.assignmentStatus.completedSimulation)
                    {
                        assigment.assignmentStatus.startedSimulation = true;
                        assigment.assignmentStatus.startedTime = DateTime.Now;
                        var split = Regex.Split(fileName, @"\.|_");
                        var id = int.Parse(split[1]);
                        System.IO.File.WriteAllText(fileName, JsonConvert.SerializeObject(assigment));
                        return new OkObjectResult(JsonConvert.SerializeObject(new AssignmentUnityAdapter(assigment,id)));
                    }
                
                }
            }

            return new OkObjectResult(false);
        }


        [HttpPost("submit_completed")]
        public IActionResult PostSubmit([FromForm] string assignmentJson)
        {
            var content = "";
            //content = GetRawText();
            var assignmentUnity = JsonConvert.DeserializeObject<AssignmentUnityAdapter>(assignmentJson);
            /*foreach (var chromosome in post.chromosomes)
            {
                content += JsonConvert.SerializeObject(chromosome);
            }*//*
            System.IO.File.WriteAllText("Data/chromosomes.txt", content);
            var chromosomes = new List<Chromosome>();
            CreateChromosome(chromosomes);
            CreateChromosome(chromosomes);
            CreateChromosome(chromosomes);
            
                
            var c = 
            */
            var id = assignmentUnity.id;
            if (System.IO.File.Exists("Data/assignment_" + id + ".json"))
            {
                var json = System.IO.File.ReadAllText("Data/assignment_" + id + ".json");

                var assigment = JsonConvert.DeserializeObject<SimulationAssigment>(json);
                foreach (var unityChromosome in assignmentUnity.chromosomes)
                {
                    var chromosome = assigment.GetChromosomeById(unityChromosome.id);
                    foreach (var simulationResult in unityChromosome.simulationResults)
                    {
                        chromosome.simulationResults.Add(simulationResult.Key,simulationResult.Value);
                    }
                    foreach (var imageResult in unityChromosome.imageResults)
                    {
                        chromosome.visualisationsResults.Add(imageResult.Key,imageResult.Value);
                    }
                }

                assigment.assignmentStatus.completedSimulation=true;
                assigment.assignmentStatus.completedTime=DateTime.Now;
                System.IO.File.WriteAllText("Data/assignment_" + id+".json", JsonConvert.SerializeObject(assigment));
            }




            var result = new OkObjectResult(true);
            
            //System.IO.File.WriteAllText("Data/test_" + id+".json", post);
            //System.IO.File.WriteAllText("Data/chromosomes.json", post.GetRawText()); //post.chromosomes.Count.ToString());
            return result;
        }
          [HttpPost("submit_Image")]

        public async Task<IActionResult> PostImage([FromForm] string image)
        {
            var png = Convert.FromBase64String(image);
            //var name = 0;
            var r =  new Random();
            var name = r.Next();

            while (System.IO.File.Exists("wwwroot/images/pic_" + name + ".png"))
            {
                
                name = r.Next();
            }
            
            //System.IO.File.Create("Data/images/pic_" + name + ".png");
            await System.IO.File.WriteAllBytesAsync("wwwroot/images/pic_" + name+".png",png);
            //System.IO.File.WriteAllText("Data/images/pic_" + name+".png",image);
            //content = GetRawText();
            
            /*foreach (var chromosome in post.chromosomes)
            {
                content += JsonConvert.SerializeObject(chromosome);
            }*//*
            System.IO.File.WriteAllText("Data/chromosomes.txt", content);
            var chromosomes = new List<Chromosome>();
            CreateChromosome(chromosomes);
            CreateChromosome(chromosomes);
            CreateChromosome(chromosomes);
            
                
            var c = 
            */
            




            var result = new OkObjectResult("/images/pic_" + name + ".png");
            
            //System.IO.File.WriteAllText("Data/test_" + id+".json", post);
            //System.IO.File.WriteAllText("Data/chromosomes.json", post.GetRawText()); //post.chromosomes.Count.ToString());
            return result;
        }
        /// <summary>
        /// stores the simulationassignment on the server and returns the id
        /// </summary>
        /// <param name="post">a simulationsassignment as JsonElement</param>
        [HttpPost]
        
        public IActionResult Post(JsonElement post)
        {
            
            var content = "";
            content = post.GetRawText();
            var assignment = JsonConvert.DeserializeObject<SimulationAssigment>(post.GetRawText());
            /*foreach (var chromosome in post.chromosomes)
            {
                content += JsonConvert.SerializeObject(chromosome);
            }
            System.IO.File.WriteAllText("Data/chromosomes.txt", content);
            var chromosomes = new List<Chromosome>();
            CreateChromosome(chromosomes);
            CreateChromosome(chromosomes);
            CreateChromosome(chromosomes);
            
                
            var c = 
            */
            var id = 0;
            while (System.IO.File.Exists("Data/assignment_" + id+".json"))
            {
                id++;
            }

            var result = new OkObjectResult(id);
            
           System.IO.File.WriteAllText("Data/assignment_" + id+".json", JsonConvert.SerializeObject(assignment));
            //System.IO.File.WriteAllText("Data/chromosomes.json", post.GetRawText()); //post.chromosomes.Count.ToString());
            return result;
        }

        /// <summary>
        /// test function that creates a random chromosome and adds it to a List
        /// </summary>
        /// <param name="chromosomes"> a list of chromosome objects to which a new chromosome is added</param>
        /*public void CreateChromosome(List<Chromosome> chromosomes)
        {
            var rand = new Random();
            var values = new int[3, 2, 3];
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    for (var k = 0; k < 3; k++)
                    {
                        values[i, j, k] = rand.Next(0, 2);
                    }
                }
            }
            chromosomes.Add(new Chromosome(values));
        }*/
    }
}