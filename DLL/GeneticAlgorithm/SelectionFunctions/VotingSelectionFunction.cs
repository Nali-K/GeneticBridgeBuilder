using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.SelectionFunctions.Interfaces;

namespace GeneticAlgorithm.SelectionFunctions
{
    public class VotingSelectionFunction:ISelectionFunction
    {
        private List<IFitnessFunctionData> _fitnessFunctions;
        public int NumberWinning => _fitnessFunctions.Sum(fitnessFunction => fitnessFunction.NumberVoteIn);

        public IConsoleController consoleController { get; set; }

        public VotingSelectionFunction(IConsoleController consoleController,
            List<IFitnessFunctionData> fitnessFunctionses)
        {
            this.consoleController = consoleController;
            _fitnessFunctions = fitnessFunctionses;

        }



        public async Task<List<IChromosome>> SelectChromosomesAsync(List<IChromosome> chromosomes, CancellationToken token)
        {

            var votedInChromosomes = new List<IChromosome>();
            var votedOutChromosomes = new List<IChromosome>();
            consoleController.LogError("start voting out");
            foreach (var fitnessFunction in _fitnessFunctions)
            {

                votedOutChromosomes.AddRange(GetVotedOut(fitnessFunction.Name,fitnessFunction.NumberVoteOut, chromosomes,fitnessFunction.RemoveInferiorChildren));
            }
            consoleController.LogError("start voting in");  

            foreach (var fitnessFunction in _fitnessFunctions)
            {

                votedInChromosomes.AddRange(GetVotedIn(fitnessFunction.Name, fitnessFunction.MinValue,fitnessFunction.NumberVoteIn, chromosomes,votedOutChromosomes));

            }
            
            consoleController.LogError("done voting in");
            return votedInChromosomes;


        }

        private static List<IChromosome> GetVotedOut(string fitnessFunctionName,int amount, IReadOnlyList<IChromosome> chromosomes,bool removeInferiorChildren=false)
        {
            var lowestList = removeInferiorChildren ? chromosomes.Where(chromosome => WorseThenParents(chromosome, fitnessFunctionName)).ToList():new List<IChromosome>();


            for (var i = 0; i < amount; i++)
            {
                IChromosome lowestChromosome = null;
                var lowestValue = float.PositiveInfinity;
                foreach (var chromosome in chromosomes)
                {
                    if (lowestList.Contains(chromosome)) continue;


                    var score = chromosome.GetScoresAndFitnessFunctionNames()[fitnessFunctionName];
                    

                    
                    
                    if (!(score < lowestValue)) continue;
                    
                    lowestChromosome = chromosome;
                    lowestValue = score;
                }
                lowestList.Add(lowestChromosome);
                
            }


            return lowestList;

        }

        private static bool WorseThenParents(IChromosome chromosome,string fitnessFunctionName)
        {
            if (chromosome.parents == null)
                return false;
            if (chromosome.parents.Count == 0)
                return false;
            foreach (var parents in chromosome.parents)
            {
                var par = parents.GetScoresAndFitnessFunctionNames();
                var chrom = chromosome.GetScoresAndFitnessFunctionNames();
                if (par == null) return false;
                if (chrom[fitnessFunctionName] >= par[fitnessFunctionName])
                    return false;
            }

            return true;
        }
        /// <summary>
        /// Get a list of chromosomes that have the highest scores in a certain fitness function
        /// </summary>
        /// <param name="fitnessFunctionName"> the name of the fitness function, used too find the correct score</param>
        /// <param name="minValue"> the minimum value for a chromosome to be added to be selected</param>
        /// <param name="amount"> the target amount of chromosomes to select, less chromosomes can be returned if to few chromosomes match the minimum value, or if too few chromosomes are provided</param>
        /// <param name="chromosomes"> the list of chromosomes to select from</param>
        /// <param name="votedOut">the list of chromosomes that have been voted out previously, those chromosomes will be excluded from selection</param>
        /// <returns>the list of chromosomes that have been "voted" to pass</returns>
        private static List<IChromosome> GetVotedIn(string fitnessFunctionName,float minValue, int amount,IReadOnlyList<IChromosome> chromosomes,IReadOnlyList<IChromosome> votedOut)
        {
            var highestList = new List<IChromosome>();
            for (var i = 0; i < amount; i++)
            {
                IChromosome highestChromosome = null;
                var highestValue = minValue;
                foreach (var chromosome in chromosomes)
                {
                    var score = chromosome.GetScoresAndFitnessFunctionNames()[fitnessFunctionName];
                    
                    if (!(score > highestValue)||highestList.Contains(chromosome)||votedOut.Contains(chromosome)) continue;
                    
                    highestChromosome = chromosome;
                    highestValue = score;
                }

                if (highestChromosome != null)
                {
                    highestList.Add(highestChromosome);
                    if (highestChromosome.ChozenBy == null)
                    {
                        highestChromosome.ChozenBy = new List<string>();
                    }
                    highestChromosome.ChozenBy.Add(fitnessFunctionName);
                }
                else
                {
                    return highestList;
                }

                
            }


            return highestList;


        }


    }
}