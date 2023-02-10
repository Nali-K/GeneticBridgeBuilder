using System.Collections.Generic;

namespace GeneticAlgorithm.Controller.Models
{
    public class EvolutionWorld
    {
        public List<Generation> generations;
        public int[] chromosomeShape;
        public float fillValueMin;
        public float fillValueMax;
        public bool chromosomesUseWholeNumbers;
        

        public EvolutionWorld(int[] chromosomeShape,float fillValueMin, float fillValueMax, bool chromosomesUseWholeNumbers)
        {
            this.chromosomeShape = chromosomeShape;
            this.fillValueMin = fillValueMin;
            this.fillValueMax = fillValueMax;
            this.chromosomesUseWholeNumbers = chromosomesUseWholeNumbers;
            generations = new List<Generation>();
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