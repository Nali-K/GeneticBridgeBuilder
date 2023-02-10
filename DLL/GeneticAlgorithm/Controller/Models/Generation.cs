using System;
using System.Collections.Generic;


namespace GeneticAlgorithm.Controller.Models
{
    public class Generation
    {
        public List<Chromosome> breedingPopulation;
        public List<Chromosome> population;
        public List<ChromosomeScores> scores;
        public DateTime startTime;
        public DateTime completedTime;
        public bool completed = false;

        public Generation(List<Chromosome> breedingPopulation)
        {
            this.breedingPopulation = breedingPopulation;

            population = new List<Chromosome>();
            scores = new List<ChromosomeScores>();
            startTime = new DateTime();
            completedTime = new DateTime();
        }
    }
}