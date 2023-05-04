using System;
using System.Collections.Generic;
using System.Deployment.Internal;


namespace GeneticAlgorithm.Controller.Models
{
    public class Generation
    {
        public int ID;
        public List<Chromosome> breedingPopulation;
        public List<Chromosome> population;

        public DateTime startTime;
        public DateTime completedTime;
        public bool completed = false;

        public Generation()
        {
            
        }
        public Generation(List<Chromosome> breedingPopulation,int ID)
        {
            this.breedingPopulation = breedingPopulation;
            this.ID = ID;
            population = new List<Chromosome>();

            startTime = new DateTime();
            completedTime = new DateTime();
        }
    }
}