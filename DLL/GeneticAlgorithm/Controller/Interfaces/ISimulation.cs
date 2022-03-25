using System;

namespace GeneticAlgorithm.Controller
{
    public interface ISimulation:IEquatable<ISimulation>
    {

        string ToJson();
        bool FromJson(string json);
    }
}