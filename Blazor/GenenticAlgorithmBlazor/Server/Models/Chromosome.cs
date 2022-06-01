using System;
using System.Collections.Generic;
using Enums;

namespace GenenticAlgorithmBlazor.Server.Models
{
    [System.Serializable]
    public class Chromosome
    {
        public int[,,] values;
        public Dictionary<SimulationResults, float> simulationResults = new Dictionary<SimulationResults, float>();
        public  int[] size= new int[3];
        public Chromosome(int[,,] values)
        {
            this.values = values;

            size[0]=values.GetLength(0);
            size[1]=values.GetLength(1);
            size[2]=values.GetLength(2);
        }
    }
}