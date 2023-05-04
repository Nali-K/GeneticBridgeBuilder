using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Controller.Models;

namespace GeneticAlgorithm.Controller.Models
{
    
    public class ChromosomeScores
    {
        //public long Chromosome { get; set; }
        public bool Selected { get; set; }
        public List<ChromosomeVisualisation> visualisations = new List<ChromosomeVisualisation>();
        public  Dictionary<string,float> Scores { get; set; }
        private readonly IConsoleController consoleController;
        public List<int> ChozenBy = new List<int>();
        
        /*public static ChromosomeScores FindByChromosome(Chromosome chromosome,List<ChromosomeScores> chromosomeScoresList,IConsoleController consoleController,bool canNull=false)
        {
            foreach (var chromsomeScore in chromosomeScoresList)
            {
                if (chromsomeScore.Chromosome == chromosome.ID)
                {
                    consoleController.LogError("Chromosome found: "+chromosome.ID);
                    return chromsomeScore;
                }
                    
               /* if(Enumerable.SequenceEqual(chromosome.DimensionSize, chromsomeScore.Chromosome.DimensionSize))
                    if (Enumerable.SequenceEqual(chromosome.GeneArray, chromsomeScore.Chromosome.GeneArray))
                    {
                        return chromsomeScore;
                    }*//*
            }
            if (!canNull)
            {
                consoleController.LogError("Chromosome not found: "+chromosome.ID);
            }
            return null;
        }*/

        public ChromosomeScores()
        {
            
        }
        public ChromosomeScores(IConsoleController consoleController)
        {
            this.consoleController = consoleController;
            Scores = new Dictionary<string, float>();
        }
        public ChromosomeVisualisation GetVisualisation(string name)
        {
            ChromosomeVisualisation vis = null;
            foreach (var visualisation in visualisations)
            {
                if (visualisation.name == name)
                {
                    if (vis != null)
                    {
                        consoleController.LogWarning("more visualisations are found with the same name, only one is returned, use GetVisualisations(name) to retrieve a list of all visualisations with this name");
                    }

                    vis = visualisation;
                }
            }

            return vis;
        }
        /*public List<ChromosomeVisualisation> GetVisualisations(string name)
        {
            List<ChromosomeVisualisation> vis = new List<ChromosomeVisualisation>();
            foreach (var visualisation in visualisations)
            {
                if (visualisation.name == name)
                {
 

                    vis.Add(visualisation);
                }
            }

            return vis;
        }*/
        public List<ChromosomeVisualisation> GetVisualisations(string visualisationFunctionName)
        {
            List<ChromosomeVisualisation> vis = new List<ChromosomeVisualisation>();
            foreach (var visualisation in visualisations)
            {
                if (visualisation.visualisationFunctionName == visualisationFunctionName)
                {
 

                    vis.Add(visualisation);
                }
            }

            return vis;
        }


        public void AddScore(string fitnessFunction, float score)
        {
            Scores[fitnessFunction] = score;
        }
        
    }
}