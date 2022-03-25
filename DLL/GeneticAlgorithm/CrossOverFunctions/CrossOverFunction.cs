using CrossOverFunctions.Interfaces;
using GeneticAlgorithm.Controller;

namespace CrossOverFunctions
{
    public abstract class CrossOverFunction
    {

         protected IConsoleController consoleController;
         
         
         public abstract IChromosome CrossOver(IChromosome[] chromosomes);
        

    }
}