using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Controller.Models;

namespace GeneticAlgorithm.Controller.Models
{
    
    public class ChromosomeScores
    {
        public Chromosome chromosome;
        public bool selected=false;
        public List<ChromosomeVisualisation> visualisations = new List<ChromosomeVisualisation>();
        private Dictionary<IFitnessFunction,float> scores= new Dictionary<IFitnessFunction, float>();
        private readonly IConsoleController consoleController;
        
        public static ChromosomeScores FindByChromosome(Chromosome chromosome,List<ChromosomeScores> chromosomeScoresList,IConsoleController consoleController,bool canNull=false)
        {
            foreach (var chromsomeScore in chromosomeScoresList)
            {
                if (chromsomeScore.chromosome == chromosome)
                    return chromsomeScore;
                if(Enumerable.SequenceEqual(chromosome.dimensionSize, chromsomeScore.chromosome.dimensionSize))
                    if (Enumerable.SequenceEqual(chromosome.geneArray, chromsomeScore.chromosome.geneArray))
                    {
                        return chromsomeScore;
                    }
            }
            if (!canNull)
            {
                consoleController.LogError("Chromosome not found");
            }
            return null;
        }
        public float GetScore(IFitnessFunction fitnessFunction)
        {
            
            return scores[fitnessFunction];
        }

        public ChromosomeScores(IConsoleController consoleController)
        {
            this.consoleController = consoleController;
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
        public List<ChromosomeVisualisation> GetVisualisations(string name)
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
        }
        public List<ChromosomeVisualisation> GetVisualisations(IVisualisationFunction visualisationFunction)
        {
            List<ChromosomeVisualisation> vis = new List<ChromosomeVisualisation>();
            foreach (var visualisation in visualisations)
            {
                if (visualisation.visualisationFunction == visualisationFunction)
                {
 

                    vis.Add(visualisation);
                }
            }

            return vis;
        }
        public List<float> GetAllScores()
        {
            var scoreList = new List<float>();
            foreach (var score in scores)
            {
                scoreList.Add(score.Value);
                
            }

            return scoreList;
        }

        public void AddScore(IFitnessFunction fitnessFunction, float score)
        {
            scores[fitnessFunction] = score;
        }
        
    }
}