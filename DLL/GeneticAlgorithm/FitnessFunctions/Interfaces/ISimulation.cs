using System;
using System.Collections.Generic;

namespace FitnessFunctions.Interfaces
{
    public interface ISimulation
    {
        List<Double> GetResults(IChromosome chromosome);
    }
}