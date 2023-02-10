using System.Collections.Generic;
using GenenticAlgorithmBlazor.Client.Controllers;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.Controller.Models;

namespace GenenticAlgorithmBlazor.Client.Models
{
    public class GenerationUIData
    {
        public int numGeneration;
        //public List<ChromosomeScores> generationChromosomes;
        public Generation generation;
        public List<string> fitnessFunctions;
        public bool opened;
        public bool openedAllChromosomes;
        public GenerationUIData(int numGeneration,Generation generation)
        {
            this.numGeneration = numGeneration;
            this.generation = generation;
            fitnessFunctions = new List<string>();
            opened = false;
            openedAllChromosomes = false;
        }

        public List<ChromosomeScores> GetSelectedChromosomes(bool not=false)
        {
            var selectedChromsomes = new List<ChromosomeScores>();
            foreach (var chromosome in generation.scores)
            {
                if ((chromosome.selected && !not) || (!chromosome.selected && not))
                {
                    selectedChromsomes.Add(chromosome);
                }
            }
            
            return selectedChromsomes;
        }



    }
}