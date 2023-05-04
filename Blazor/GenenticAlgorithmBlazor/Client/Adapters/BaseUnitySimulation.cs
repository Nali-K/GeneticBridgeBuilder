using System;
using System.Collections.Generic;
using System.Linq;
using GenenticAlgorithmBlazor.Shared;
using Newtonsoft.Json;


using IChromosome = GeneticAlgorithm.FitnessFunctions.Interfaces.IChromosome;

namespace Adapters
{
    public abstract class BaseUnitySimulation
    {
        protected List<SharedChromosome> resultChromosomes = new List<SharedChromosome>();
        protected List<SimulationAssigment> GetAssignment(string evolutionWorldName, List<IChromosome> chromosomes,string simulationName)
        {

            var size = (double)chromosomes.Count * chromosomes[0].geneArray.Length;
            var parts = (int)Math.Ceiling(size / 150000d);
            var chromosomesPerPart= (int)Math.Ceiling((double)chromosomes.Count / parts);
            var assignements = new List<SimulationAssigment>();
            for (var i = 0; i < parts; i++)
            {
                var assignement = new SimulationAssigment();
                var testlist = new List<IChromosome>();
                var testdict = new Dictionary<IChromosome, string>();
                assignement.evolutionWorldName = evolutionWorldName;
                var addedNew = false;
                for (var j=i*chromosomesPerPart;j<(i+1)*chromosomesPerPart&&j<chromosomes.Count;j++)
                {
                    var chromosome = chromosomes[j];
                    var chomsomeIsNew = true;
                    foreach (var c in resultChromosomes.Where(c => c.ID == chromosome.ID))
                    {
                        chomsomeIsNew = false;
                        break;
                    }

                    if (chomsomeIsNew)
                    {
                        assignement.chromosomes.Add(chromosome as SharedChromosome);
                        testlist.Add(chromosome);
                        testdict.Add(chromosome, "test");
                        addedNew = true;
                    }
                }

                if (!addedNew) continue;
                assignement.Simulation = simulationName;

                TestText testText = new();
                var settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.None;


                testText.testText = "test1";
                assignements.Add(assignement);
            }


            return assignements;
        }
    }
}