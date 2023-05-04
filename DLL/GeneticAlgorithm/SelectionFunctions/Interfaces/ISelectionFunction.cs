using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.SelectionFunctions.Interfaces;

namespace GeneticAlgorithm.SelectionFunctions
{
    public interface ISelectionFunction
    {
        int NumberWinning
        {
            get;
        }
        
        IConsoleController consoleController { get; set; }
        Task<List<IChromosome>> SelectChromosomesAsync(List<IChromosome> chromosomes,CancellationToken token);

    }
}