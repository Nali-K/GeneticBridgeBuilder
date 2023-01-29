using System;
using System.Collections.Generic;
using System.Diagnostics;
using Enums;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.FitnessFunctions.Enums;
using GeneticAlgorithm.FitnessFunctions.Interfaces;

namespace GenenticAlgorithmBlazor.Shared
{
    public class SimulationAssigment
    {

        public SimulationAssigment()
        {
            assignmentStatus = new AssignmentStatus();
            chromosomes = new List<SharedChromosome>();
        }

        public string Simulation { get; set; }
        public AssignmentStatus assignmentStatus { get; set; }
        public List<SharedChromosome> chromosomes { get; set; }

        public SharedChromosome GetChromosomeById(int id)
        {
            if (chromosomes[id].id == id)
                return chromosomes[id];
            foreach (var chromosome in chromosomes)
            {
                if (chromosome.id == id)
                    return chromosome;
            }

            Console.WriteLine("error: can't find chromosome id");
            return chromosomes[0];
        }
    }


}