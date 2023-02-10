using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Simulation
{
    [System.Serializable]
    public class Chromosome
    {
        public int[,,] values;
        public Dictionary<string, float> simulationResults = new Dictionary<string, float>();
        public Dictionary<string, string> imageResults = new Dictionary<string, string>();
        public  int[] size= new int[3];
        public int id;
        public Chromosome(int[,,] values)
        {
            this.values = values;

            size[0]=values.GetLength(0);
            size[1]=values.GetLength(1);
            size[2]=values.GetLength(2);
        }
    }
}