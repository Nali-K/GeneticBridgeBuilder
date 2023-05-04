using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GenenticAlgorithmBlazor.Server.Models;
using GenenticAlgorithmBlazor.Shared;
using GeneticAlgorithm.Controller.Models;
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
         public static int AtAssignment = 0;
         public static Dictionary<int,SimulationAssigment> Assignments = new Dictionary<int,SimulationAssigment>();
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

           if (!Assignments.ContainsKey(id)) return new OkObjectResult(false);
           if (!Assignments[id].assignmentStatus.completedSimulation) return new OkObjectResult(false);
           var json = JsonConvert.SerializeObject(Assignments[id]);
           Assignments.Remove(id);
           return new OkObjectResult(json);
        }
        [HttpGet("get_free")]
        public IActionResult Get()
        {

            //return System.IO.File.ReadAllText("Data/test.txt");
            foreach (var assignment in Assignments)
            {
                
                /*if (Regex.IsMatch(fileName, "(assignment_[0-9]+.json)")&&System.IO.File.Exists(fileName))
                {
                    var split = Regex.Split(fileName, @"\.|_");
                    var id = int.Parse(split[1]);
                    if (_invalidAssignments.Contains(id)) continue;
                    _invalidAssignments.Add(id);
                    var json = System.IO.File.ReadAllText(fileName);
                    var assigment = JsonConvert.DeserializeObject<SimulationAssigment>(json);
                    if (assigment == null)
                    {
                        return new OkObjectResult(false);
                    }*/
                    if (!assignment.Value.assignmentStatus.startedSimulation&&!assignment.Value.assignmentStatus.completedSimulation)
                    {
                        assignment.Value.assignmentStatus.startedSimulation = true;
                        assignment.Value.assignmentStatus.startedTime = DateTime.Now;


                        return new OkObjectResult(JsonConvert.SerializeObject(new AssignmentUnityAdapter(assignment.Value,assignment.Key)));
                    }
                    
                    
                
                //}
            }

            return new OkObjectResult(false);
        }

        [HttpGet("get_save_id/{name}")]
        public IActionResult GetSave(string name)
        {

            if (System.IO.File.Exists("Data/saves/"+name+"/ew-"+name+".json"))
            {
                return new OkObjectResult(System.IO.File.ReadAllText("Data/saves/"+name+"/ew-"+name+".json"));
            }

            return new OkObjectResult(JsonConvert.SerializeObject(""));
        }
        [HttpGet("get_save_gen_id/{name}/{ID}")]
        public IActionResult GetSaveGen(string name,string ID)
        {

            if (System.IO.File.Exists("Data/saves/"+name+"/"+ID+".json"))
            {
                return new OkObjectResult(System.IO.File.ReadAllText("Data/saves/"+name+"/"+ID+".json"));
            }

            return new OkObjectResult(JsonConvert.SerializeObject(""));
        }
        [HttpGet("get_all_saves")]
        public IActionResult GetAllSaves()
        {
           /* if (System.IO.File.Exists("Data/saves/logs.json"))
            {
                return new OkObjectResult(System.IO.File.ReadAllText("Data/saves/logs.json"));
            }*/

            //return new OkObjectResult(JsonConvert.SerializeObject(new Dictionary<long,int>()));
            var returnList = new List<string>();
            foreach (var fileName in System.IO.Directory.EnumerateDirectories("Data/saves"))
            {
                //if (Regex.IsMatch(fileName, "ew-[a-zA-Z0-9_]+.json"))
                if (Regex.IsMatch(fileName, "[a-zA-Z0-9_]"))
                {
                    /*var s = System.IO.File.ReadAllText(fileName);
                    if (s.Length>0)
                    {*/
                        var split = Regex.Split(fileName, @"\\");

                        returnList.Add(split[1]);
                    //}
                }

            }

            return new OkObjectResult(JsonConvert.SerializeObject(returnList));
        }
        [HttpGet("get_new_save/{name}")]
        public IActionResult GetNewSave(string name)
        {
            return !Regex.IsMatch(name, "([a-zA-Z0-9_])") ? new OkObjectResult(false) : new OkObjectResult(!System.IO.Directory.Exists("Data/saves/" + name ));
        }
        [HttpPost("submit_save")]
        public IActionResult PostSave(JsonElement saveJson)
        {
            //var content = "";
            //content = GetRawText();

            var save = JsonConvert.DeserializeObject<EvolutionWorldWrapper>(saveJson.GetRawText());
            Console.WriteLine(saveJson);
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
            if (!System.IO.Directory.Exists("Data/saves/" + save.name))
                Directory.CreateDirectory("Data/saves/" + save.name);

            System.IO.File.WriteAllText("Data/saves/"+save.name+"/ew-"+save.name+".json",save.json);
            /*       Dictionary<long, int> saves;

       if (System.IO.File.Exists("Data/saves/logs.json"))
       {
           saves = JsonConvert.DeserializeObject<Dictionary<long,int>>(System.IO.File.ReadAllText("Data/saves/logs.json"));
/*
           oreach (var s in saves)
           {
               if (s.ID == id)
               {
                   
                   break;
               }
           }*//*
            }
            else
            {
                saves = new Dictionary<long, int>();
            }
            if (saves.ContainsKey(id))
            {
                saves[id]=JsonConvert.DeserializeObject<EvolutionWorld>(save.json).generations.Count;
            }
            else
            {
                saves.Add(id,JsonConvert.DeserializeObject<EvolutionWorld>(save.json).generations.Count);
            }
            System.IO.File.WriteAllText("Data/saves/logs.json",JsonConvert.SerializeObject(saves));

*/



                var result = new OkObjectResult(true);
            
            //System.IO.File.WriteAllText("Data/test_" + id+".json", post);
            //System.IO.File.WriteAllText("Data/chromosomes.json", post.GetRawText()); //post.chromosomes.Count.ToString());
            return result;
        }
                [HttpPost("submit_save_generation")]
        public IActionResult PostSaveGen(JsonElement saveJson)
        {
            //var content = "";
            //content = GetRawText();

            var save = JsonConvert.DeserializeObject<GenerationWrapper>(saveJson.GetRawText());
            Console.WriteLine(saveJson);
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
            if (System.IO.Directory.Exists("Data/saves/" + save.EvolutionWorld)){

                System.IO.File.WriteAllText("Data/saves/" + save.EvolutionWorld + "/" + save.ID + ".json", save.json);
            }
          




                var result = new OkObjectResult(true);
            
            //System.IO.File.WriteAllText("Data/test_" + id+".json", post);
            //System.IO.File.WriteAllText("Data/chromosomes.json", post.GetRawText()); //post.chromosomes.Count.ToString());
            return result;
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
            if (Assignments.ContainsKey(id))
            {
   

                var assigment = Assignments[id];
                foreach (var unityChromosome in assignmentUnity.chromosomes)
                {
                    var unityChromosomeUnPacked = unityChromosome.ToCromosome();
                    var chromosome = assigment.GetChromosomeById(unityChromosome.id);
                    foreach (var simulationResult in unityChromosomeUnPacked.simulationResults)
                    {
                        chromosome.simulationResults.Add(simulationResult.Key,simulationResult.Value);
                    }
                    foreach (var imageResult in unityChromosomeUnPacked.imageResults)
                    {
                        chromosome.visualisationsResults.Add(imageResult.Key,imageResult.Value);
                    }
                }

                assigment.assignmentStatus.completedSimulation=true;
                assigment.assignmentStatus.completedTime=DateTime.Now;
                //System.IO.File.WriteAllText("Data/assignment_" + id+".json", JsonConvert.SerializeObject(assigment));
            }




            var result = new OkObjectResult(true);
            
            //System.IO.File.WriteAllText("Data/test_" + id+".json", post);
            //System.IO.File.WriteAllText("Data/chromosomes.json", post.GetRawText()); //post.chromosomes.Count.ToString());
            return result;
        }
          [HttpPost("submit_Image/{assignmentId}")]

        public async Task<IActionResult> PostImage([FromForm] string image,string assignmentId)
        {
            var png = Convert.FromBase64String(image);
            //var name = 0;
            var r =  new Random();
            var name = r.Next();

            var id = 0;
            if (int.TryParse(assignmentId, out id))
            {

                if (Assignments.ContainsKey(id))
                {
                    var url = "wwwroot/images/" + Assignments[id].evolutionWorldName;
                    if (!Directory.Exists(url))
                    {
                        Directory.CreateDirectory(url);
                    }

                    while (System.IO.File.Exists(url + "/pic_" + name + ".png"))
                        {

                            name = r.Next();
                        }

                        url+="/pic_" + name + ".png";

                        //System.IO.File.Create("Data/images/pic_" + name + ".png");
                        await System.IO.File.WriteAllBytesAsync(url, png);
                        //System.IO.File.WriteAllText("Data/images/pic_" + name+".png",image);
                        //content = GetRawText();
                        url = Regex.Replace(url, "wwwroot/", "");
                        /*foreach (var chromosome in post.chromosomes)
                        {
                            content += JsonConvert.SerializeObject(chromosome);
                        }*/ /*
            System.IO.File.WriteAllText("Data/chromosomes.txt", content);
            var chromosomes = new List<Chromosome>();
            CreateChromosome(chromosomes);
            CreateChromosome(chromosomes);
            CreateChromosome(chromosomes);
            
                
            var c = 
            */



                        var result = new OkObjectResult(url);
                        return result;
                    
                }
            }

            var result2 = new OkObjectResult(null);
            
            //System.IO.File.WriteAllText("Data/test_" + id+".json", post);
            //System.IO.File.WriteAllText("Data/chromosomes.json", post.GetRawText()); //post.chromosomes.Count.ToString());
            return result2;
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
            var id = AtAssignment;
                AtAssignment++;
            Assignments.Add(id,assignment);
            
            var result = new OkObjectResult(id);

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