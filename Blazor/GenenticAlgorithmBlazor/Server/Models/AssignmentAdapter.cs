using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using GenenticAlgorithmBlazor.Shared;

namespace GenenticAlgorithmBlazor.Server.Models
{
    public class AssignmentUnityAdapter
    {
        public bool completed;
        public List<String> simulationsToRun = new List<String>();
        public List<JsonChromosomeUnityAdapter> chromosomes= new List<JsonChromosomeUnityAdapter>();
        public int id;

        public AssignmentUnityAdapter()
        {
            
        }
        public AssignmentUnityAdapter(SimulationAssigment assigment,int id)
        {
            this.id = id;
            completed = assigment.assignmentStatus.completedSimulation;
            simulationsToRun = new List<string>();
            simulationsToRun.Add(assigment.Simulation);
            foreach (var chromosome in assigment.chromosomes)
            {
                if (chromosome.numDimentions != 3)
                {
                    throw new System.Exception();
                }

                var values = new int[chromosome.dimensionSize[0], chromosome.dimensionSize[1],
                    chromosome.dimensionSize[2]];
                
                var genes = chromosome.geneArray;
                for (var i=0; i<genes.Length;i++)
                {
                    values[i % chromosome.dimensionSize[0],
                        (i / chromosome.dimensionSize[0])% chromosome.dimensionSize[1], 
                        i / (chromosome.dimensionSize[0] * chromosome.dimensionSize[1])] = (int)genes[i];
                }

                var unityChromsome = new ChromosomeUnityAdapter(values)
                {
                    id = (int)chromosome.ID
                };
                foreach (var result in chromosome.simulationResults)
                {
                    unityChromsome.simulationResults.Add(result.Key,result.Value);
                }
                chromosomes.Add(new JsonChromosomeUnityAdapter(unityChromsome));
                
            }
        }
    }
}