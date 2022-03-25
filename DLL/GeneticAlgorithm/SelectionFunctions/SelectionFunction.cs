using System;
using System.Collections.Generic;
using SelectionFunctions.Controller;
using SelectionFunctions.Interfaces;

namespace SelectionFunctions
{
    public abstract class SelectionFunction
    {
        protected  IConsoleController consoleController;
        public abstract List<IChromosome> SelectChromosomes(List<IChromosome> chromosomes);
        public abstract Dictionary<string, string> GetParameters();
        public abstract bool SetParameters(Dictionary<string, string> parameters);
    }
}