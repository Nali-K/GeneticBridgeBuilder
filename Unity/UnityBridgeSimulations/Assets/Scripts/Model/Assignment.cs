using System.Collections.Generic;
using Enums;

namespace Simulation
{
    public class Assignment
    {
        public bool completed;
        public List<Simulations> simulationToRun= new List<Simulations>();
        public List<Chromosome> chromosomes= new List<Chromosome>();
    }
}