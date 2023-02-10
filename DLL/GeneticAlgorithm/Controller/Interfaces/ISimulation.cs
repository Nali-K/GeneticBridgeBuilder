using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller.Models;

namespace GeneticAlgorithm.Controller
{
    public interface ISimulation:IEquatable<ISimulation>
    {

        string ToJson();
        bool FromJson(string json);
        Task<bool[]> RunSimulationAsync(List<Chromosome> chromosome);
        void ResetResults();
    }
}