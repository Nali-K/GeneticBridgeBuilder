using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Controller
{
    public interface IFitnessFunction
    { 
        Task<ISimulation> GetRequiredSimulation();
        Type GetRequiredSimulationType();
        
        string ToJson();
        bool FromJson(string json);
    }
}