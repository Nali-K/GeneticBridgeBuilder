using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.SelectionFunctions.Interfaces;

namespace GeneticAlgorithm.SelectionFunctions
{
    public abstract class SelectionFunction
    {
        protected  IConsoleController consoleController;
        public abstract Task<List<IChromosome>> SelectChromosomesAsync(List<IChromosome> chromosomes,CancellationToken token);
        public abstract Dictionary<string, string> GetParameters();
        public abstract int GetNumberExpectedWinners();
        public abstract bool SetParameters(Dictionary<string, string> parameters);
    }
}