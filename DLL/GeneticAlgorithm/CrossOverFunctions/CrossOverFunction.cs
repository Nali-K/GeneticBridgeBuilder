using GeneticAlgorithm.CrossOverFunctions.Interfaces;

namespace GeneticAlgorithm.CrossOverFunctions
{
    public abstract class CrossOverFunction
    {

         protected IConsoleController consoleController;
         
         
         public abstract IChromosome[] CrossOver(IChromosome[] chromosomes);
        

    }
}