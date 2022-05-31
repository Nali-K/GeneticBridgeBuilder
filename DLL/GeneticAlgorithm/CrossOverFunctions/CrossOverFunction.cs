using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.CrossOverFunctions.Interfaces;

namespace GeneticAlgorithm.CrossOverFunctions
{
    public abstract class CrossOverFunction
    {

         protected IConsoleController consoleController;
         
         
         public abstract Task<IChromosome[]> CrossOverAsync(IChromosome[] chromosomes,CancellationToken token);
        

    }
}