using System;
using GeneticAlgorithm.FitnessFunctions.Interfaces;

namespace GenenticAlgorithmBlazor.Shared
{
    public class AssignmentStatus
    {



        public DateTime createdTime { get; set; }
        public bool startedSimulation { get; set; }
        public DateTime startedTime { get; set; }
        public bool completedSimulation { get; set; }
        public DateTime completedTime { get; set; }
        public AssignmentStatus()
        {
            createdTime = DateTime.Now;
            startedSimulation = false;
            completedSimulation = false;
        }
    }
}