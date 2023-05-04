using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.CrossOverFunctions.Interfaces;

namespace GeneticAlgorithm.CrossOverFunctions
{
    [Serializable]
    public class RandomRiffle:CrossOverFunction
    {
        private int acrossDimension;
        private bool fillRandom;
        private float fillValue;
        private float fillValueMin;
        private float fillValueMax;
        private bool selfCrossOver;
        private bool chozenByCrossOver;

        public RandomRiffle(int acrossDimension,IConsoleController consoleController, bool chozenByCrossOver=true, float fillValue = 0f,bool selfCrossOver=true)
        {
            this.acrossDimension = acrossDimension;
            this.chozenByCrossOver = chozenByCrossOver;
            fillRandom = false;
            this.fillValue = fillValue;
            this.consoleController = consoleController;
            this.selfCrossOver = selfCrossOver;
        }

        public RandomRiffle(int acrossDimension, float fillValueMin, float fillValueMax,IConsoleController consoleController, bool chozenByCrossOver=true, bool selfCrossOver=true)
        {
            this.acrossDimension = acrossDimension;
            fillRandom = true;
            this.fillValueMin = fillValueMin;
            this.fillValueMax = fillValueMax;
            this.chozenByCrossOver = chozenByCrossOver;
            this.consoleController = consoleController;
            this.selfCrossOver = selfCrossOver;
        }

        public override async Task<IChromosome[]> CrossOverAsync(IChromosome[] chromosomes,CancellationToken token)
        {

            var chozenByTheSame = 0;
            var chozenByTheDifferent = 0;
            consoleController.LogMessage("randomRiffle chromosomeslenght: "+chromosomes);
            var newChromosomes = new List<IChromosome>();
            for (var i = 0; i < chromosomes.Length; i++)
            {
                for (var j = 0; j < chromosomes.Length; j++)
                {
                    
                    if ((selfCrossOver||i!=j)&&(chozenByCrossOver||chromosomes[i].ChozenBy==-1||chromosomes[i].ChozenBy!=chromosomes[j].ChozenBy)) newChromosomes.Add(GetCrossOver(new[] { chromosomes[i],chromosomes[j]}));
                    if (chozenByCrossOver || chromosomes[i].ChozenBy == -1 ||
                        chromosomes[i].ChozenBy != chromosomes[j].ChozenBy) chozenByTheDifferent++;
                    else
                    {
                        chozenByTheSame++;
                    }
                    //consoleController.LogMessage("---  "+chromosomes[i].ChozenBy+"  ---  "+chromosomes[j].ChozenBy+"  ---");
                }
                await Task.Delay(5,token);
            }
            consoleController.LogMessage("chozen by same: "+chozenByTheSame);
            consoleController.LogMessage("chozen by different: "+chozenByTheDifferent);
            return newChromosomes.ToArray();
        }
        private IChromosome GetCrossOver(IChromosome[] chromosomes)
        {
            if (chromosomes.Length != 2)
            {
                consoleController.LogError("Simple cut Merger requires precicly two chromosons");
                return null;
            }

            var c1 = chromosomes[0];
            var c2 = chromosomes[1];
            if (c1.GetNumDimensions() != c2.GetNumDimensions())
            {
                consoleController.LogError("Incompatible dimensions");
                return null;
            }

            var newGeneArraySize = calculateNewGeneArraySize(c1, c2);
            //consoleController.LogError(newGeneArraySize[0].ToString()+newGeneArraySize[1].ToString());
            var newChromosome = c1.CreateNewChromosome(newGeneArraySize);
            if (fillRandom)
            {
                newChromosome.Fill(fillValueMin,fillValueMax);
            }
            else
            {
                var fillValues =new float[newChromosome.GetGeneArray().Length];
                for (int i = 0; i < fillValues.Length; i++)
                {
                    fillValues[i] = fillValue;
                }
                newChromosome.Fill(fillValues);
                
            }

            var pos = new int[c1.GetNumDimensions()];
            for (var i = 0; i < c1.GetNumDimensions(); i++)
            {
                pos[i] = -1;
            }

            var random = new Random();
            
            for (var i = 0; i < c1.GetDimensionSize(acrossDimension); i++)
            {
                pos[acrossDimension] = i;

                var val = random.Next(2) == 0 ? c1.GetValuesAndPositions(pos) : c2.GetValuesAndPositions(pos);

                newChromosome.InsertValues(val);
            }

           

            newChromosome.Ancestors = chromosomes.ToList();
            return newChromosome;
        }




        private int[] calculateNewGeneArraySize(IChromosome c1, IChromosome c2)
        {
            var newGeneArraySize = new int[c1.GetNumDimensions()];
            for (var i = 0; i < c1.GetNumDimensions(); i++)
            {
                if (i == acrossDimension)
                {
                    newGeneArraySize[i] = (int)Math.Ceiling(c1.GetDimensionSize(i) / 2d);
                    newGeneArraySize[i] += (int)Math.Floor(c2.GetDimensionSize(i) / 2d);
                }
                else
                {
                    if (c1.GetDimensionSize(i) > c2.GetDimensionSize(i))
                    {
                        newGeneArraySize[i] = c1.GetDimensionSize(i);
                    }
                    else
                    {
                        newGeneArraySize[i] =c2.GetDimensionSize(i);
                    }
                }
            }

            return newGeneArraySize;
        }
    }
}