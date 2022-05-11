﻿using System;
using System.Collections.Generic;

using GeneticAlgorithm.SelectionFunctions.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace GeneticAlgorithm.SelectionFunctions
{
    public class AddScores:SelectionFunction
    {
        private int numberWinning=5;
        public AddScores(IConsoleController consoleController,int numberWinning)
        {
            this.consoleController = consoleController;
            this.numberWinning = numberWinning;
        }

        public override async Task<List<IChromosome>> SelectChromosomesAsync(List<IChromosome> chromosomes)
        {
            var d = new Dictionary<IChromosome, float>();
            foreach (var chromosome in chromosomes)
            {
                var totalScore = 0f;
                foreach (var score in chromosome.GetScores())
                {
                    totalScore += score;
                }
                d.Add(chromosome,totalScore);
            }


            var sortedDict = from entry in d orderby entry.Value descending select entry;
     
            List<IChromosome> winners = new List<IChromosome>();
            var correctedNumberWinning = numberWinning;
            if (numberWinning >= sortedDict.Count())
            {
                correctedNumberWinning = sortedDict.Count();
                consoleController.LogWarning("number of winners is larger then or equal to the amount of candidate, meaning every chromosome in will be selected");
            }
            for (var i = 0; i < correctedNumberWinning; i++)
            {
                var c = sortedDict.ElementAt(i);
                winners.Add(c.Key);
            }

            return winners;
        }

        public override Dictionary<string, string> GetParameters()
        {
            var parameterDictionary = new Dictionary<string, string>();
            parameterDictionary.Add("Number to select",numberWinning.ToString());
            return parameterDictionary;
        }

        public override bool SetParameters(Dictionary<string, string> parameters)
        {
            if (!parameters.ContainsKey("Number to select"))
            {
                consoleController.LogError("invalid parameters");
                return false;
            }

            if (!int.TryParse(parameters["Number to select"], out numberWinning))
            {
                consoleController.LogError("invalid parameters");
                return false;
            }

            return true;
        }
    }
}