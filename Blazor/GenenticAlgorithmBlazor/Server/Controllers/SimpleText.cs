using System;
using System.Collections.Generic;
using System.Linq;
using GenenticAlgorithmBlazor.Server.Models;
using GenenticAlgorithmBlazor.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GenenticAlgorithmBlazor.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SimpleText:ControllerBase
    {
        private readonly ILogger<SimpleText> _logger;

        public SimpleText(ILogger<SimpleText> logger)
        {
            _logger = logger;
        }

        //public void Write(string text)
        //{
        //    System.IO.File.WriteAllText("Data/test.txt", text);
        //}
        [HttpGet]
        public string Get()
        {

            return System.IO.File.ReadAllText("Data/test.txt");
            
        }

        /// <summary>
        /// store a simple text in a data folder and store 3 random chromosomes in a data folder as test
        /// </summary>
        /// <param name="post">the simple text</param>
        [HttpPost]
        
        public void Post(TestText post)
        {
            System.IO.File.WriteAllText("Data/test.txt", post.testText);
            var chromosomes = new List<SharedChromosome>();
            //CreateChromosome(chromosomes);
            //CreateChromosome(chromosomes);
            //CreateChromosome(chromosomes);
            var c = JsonConvert.SerializeObject(chromosomes);
            System.IO.File.WriteAllText("Data/chromosomes.json", c);

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