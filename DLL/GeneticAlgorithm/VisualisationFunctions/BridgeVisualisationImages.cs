using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.VisualisationFunctions.Interfaces;

namespace GeneticAlgorithm.VisualisationFunctions
{
   [Serializable] public class BridgeVisualisationImages:VisualisationFunction
    {
        public IImageSimulation simulator;

        


        
        public BridgeVisualisationImages( IImageSimulation simulator)
        {
            this.simulator = simulator;

        }
        
        public override async Task<Dictionary<IChromosome,Dictionary<string,string>>> VisualiseAsync(List<IChromosome> chromosomes, CancellationToken token)
        {/*
            var inst =Object.Instantiate(simulator,position,Quaternion.identity);
            var done = false;
            var simulation = inst.GetComponent<Simulation.Simulation>();
       */
            var outputDict = new Dictionary<IChromosome, Dictionary<string, string>>();
     
            await simulator.SimulateAsync(chromosomes);

            foreach (var chromosome in chromosomes)
            {
                var result = simulator.GetImages(chromosome);
                outputDict.Add(chromosome,result);

            }

            return outputDict;
        }


    }
}