using System;
using System.Collections.Generic;
using System.Diagnostics;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.Controller.Models;

namespace Adapters
{
    public class AdapterSelectionChromosome:GeneticAlgorithm.SelectionFunctions.Interfaces.IChromosome
    {
        ChromosomeScores scores;
        private Dictionary<IFitnessFunction, string> fitnessFunctionNames;
        public AdapterSelectionChromosome(ChromosomeScores scores,Dictionary<IFitnessFunction,string> fitnessFunctionNames)
        {
            this.scores = scores;
            this.fitnessFunctionNames = fitnessFunctionNames;
        }
        public List<float> GetScores()
        {
            return scores.GetAllScores();
        }

        public Dictionary<string, float> GetScoresAndFitnessFunctionNames()
        {
            var returnDict = new Dictionary<string, float>();
            foreach (var VARIABLE in fitnessFunctionNames)
            {
                returnDict.Add(VARIABLE.Value,scores.GetScore(VARIABLE.Key));
            }

            return returnDict;
        }

        public Chromosome GetChromosome()
        {
            return scores.chromosome;
        }
    }
}