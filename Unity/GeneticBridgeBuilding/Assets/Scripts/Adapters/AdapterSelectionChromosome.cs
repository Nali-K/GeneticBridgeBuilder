using System.Collections.Generic;
using GeneticAlgorithm.Controller;

namespace Adapters
{
    public class AdapterSelectionChromosome:GeneticAlgorithm.SelectionFunctions.Interfaces.IChromosome
    {
        ChromosomeScores scores;
        GeneticAlgorithm.Controller.Chromosome chromosome;
        public AdapterSelectionChromosome(GeneticAlgorithm.Controller.ChromosomeScores scores,GeneticAlgorithm.Controller.Chromosome chromosome)
        {
            this.scores = scores;
            this.chromosome = chromosome;
        }
        public List<float> GetScores()
        {
            return scores.GetAllScores();
        }

        public GeneticAlgorithm.Controller.Chromosome GetChromosome()
        {
            return chromosome;
        }
    }
}