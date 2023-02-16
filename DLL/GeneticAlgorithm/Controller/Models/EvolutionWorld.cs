using System.Collections.Generic;

namespace GeneticAlgorithm.Controller.Models
{
    public class EvolutionWorld
    {
        public List<Generation> generations;
        public ChromosomeBaseData ChromosomeBaseData;
        public long AtChromosomeID => atChromosomeID++;
        private long atChromosomeID;

        public EvolutionWorld(int[] chromosomeShape,ChromosomeBaseData chromosomeBaseData)
        {
            atChromosomeID = 0;
            generations = new List<Generation>();
            this.ChromosomeBaseData = chromosomeBaseData;
        }
        

        public Generation GetCurrentGeneration()
        {
            return generations[generations.Count - 1];
        }

        public int NumberFinishedGenerations()
        {
            if (GetCurrentGeneration().completed)
            {
                return generations.Count;
            }
            else
            {
                return generations.Count - 1;
            }
        }
        
    }
}