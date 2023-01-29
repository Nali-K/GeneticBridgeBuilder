using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Simulation
{
    public class Assignment
    {
        public bool completed;
        public List<String> simulationsToRun = new List<String>();
        public List<Chromosome> chromosomes= new List<Chromosome>();
        public int id;
        public static Simulations GetSimulation(string sim)
        {
            sim=sim.ToLower();
            switch (sim)
            {
                case "dropblock":
                    return Simulations.Dropblock;
                case "stability":
                    return Simulations.Stability;
                default:
                    Debug.LogError("unknown simulation, returning dropblock-simulation instead");
                    return Simulations.Dropblock;
            }
        }
    }
   
    
}