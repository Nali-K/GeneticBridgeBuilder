using System;
using System.Collections.Generic;

using GeneticAlgorithm.SelectionFunctions.Interfaces;

namespace GeneticAlgorithm.SelectionFunctions
{
    public abstract class SelectionFunction
    {
        protected  IConsoleController consoleController;
        public abstract List<IChromosome> SelectChromosomes(List<IChromosome> chromosomes);
        public abstract Dictionary<string, string> GetParameters();
        public abstract bool SetParameters(Dictionary<string, string> parameters);
    }
}