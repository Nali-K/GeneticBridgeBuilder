using System;
using System.Collections.Generic;

using GeneticAlgorithm.SelectionFunctions.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeneticAlgorithm.SelectionFunctions
{
    public class AddScoresNormalised:ISelectionFunction
    {
        public int NumberWinning { get; } = 5;
        private Dictionary<string, float> minValues;
        private Dictionary<string, float> maxValues;
        public IConsoleController consoleController { get; set; }
        public AddScoresNormalised(IConsoleController consoleController,int numberWinning)
        {
            this.consoleController = consoleController;
            this.NumberWinning = numberWinning;
        }
        public async Task<List<IChromosome>> SelectChromosomesAsync(List<IChromosome> chromosomes,CancellationToken token)
        {
            var d = new Dictionary<IChromosome, float>();
            SetNormalisation(chromosomes);
            foreach (var chromosome in chromosomes)
            {
                var totalScore = 0f;
              //  var str = "";
                foreach (var score in NormalizeChromosomeScores(chromosome))
                {
                   // str += score + "\t\t";
                    totalScore += score;
                }
                d.Add(chromosome,totalScore);
                //consoleController.LogMessage(str);
            }


            var sortedDict = from entry in d orderby entry.Value descending select entry;
     
            List<IChromosome> winners = new List<IChromosome>();
            var correctedNumberWinning = NumberWinning;
            if (NumberWinning >= sortedDict.Count())
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

        private void SetNormalisation(List<IChromosome> chromosomes)
        {
            minValues = new Dictionary<string, float>();
            maxValues = new Dictionary<string, float>();
            foreach (var scores in chromosomes.SelectMany(chromosome => chromosome.GetScoresAndFitnessFunctionNames()))
            {
                if (minValues.ContainsKey(scores.Key))
                {
                    if (scores.Value < minValues[scores.Key])
                    {
                        minValues[scores.Key] = scores.Value;
                    }

                }else
                {
                    minValues.Add(scores.Key,scores.Value);
                }
                if (maxValues.ContainsKey(scores.Key))
                {
                    if (scores.Value >maxValues[scores.Key])
                    {
                        maxValues[scores.Key] = scores.Value;
                    }

                }else
                {
                    maxValues.Add(scores.Key,scores.Value);
                }
            }
        }
        private List<float> NormalizeChromosomeScores(IChromosome chromosome)
        {
            var returnList = new List<float>();
            foreach (var scores in chromosome.GetScoresAndFitnessFunctionNames())
            {
                var r = scores.Value - minValues[scores.Key];
                if (maxValues[scores.Key] - minValues[scores.Key] == 0)
                {
                    consoleController.LogError("div by 0");
                }
                r /= maxValues[scores.Key] - minValues[scores.Key];
                returnList.Add(r);
            }

            return returnList;
        } 
        
        /*
        public override Dictionary<string, string> GetParameters()
        {
            var parameterDictionary = new Dictionary<string, string>();
            parameterDictionary.Add("Number to select",numberWinning.ToString());
            return parameterDictionary;
        }
        public override int GetNumberExpectedWinners()
        {
            return numberWinning;
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
        }*/
    }
}