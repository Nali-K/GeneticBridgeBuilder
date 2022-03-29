using System;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Controller
{
    public interface ISimulation:IEquatable<ISimulation>
    {

        string ToJson();
        bool FromJson(string json);
        Task<bool> RunSimulation(Chromosome chromosome);
        void ResetResults();
    }
}