using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Controller
{
    public interface ISimulation:IEquatable<ISimulation>
    {

        string ToJson();
        bool FromJson(string json);
        Task<bool[]> RunSimulation(List<Chromosome> chromosome);
        void ResetResults();
    }
}