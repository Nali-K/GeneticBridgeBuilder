using System;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.CrossOverFunctions.Interfaces;

namespace GeneticAlgorithm.CrossOverFunctions
{
    [Serializable]
    public class SimpleCut:CrossOverFunction
    {
        private int acrossDimension;
        private bool fillRandom;
        private float fillValue;
        private float fillValueMin;
        private float fillValueMax;

        public SimpleCut(int acrossDimension,IConsoleController consoleController, float fillValue = 0f)
        {
            this.acrossDimension = acrossDimension;
            fillRandom = false;
            this.fillValue = fillValue;
            this.consoleController = consoleController;
        }

        public SimpleCut(int acrossDimension, float fillValueMin, float fillValueMax,IConsoleController consoleController)
        {
            this.acrossDimension = acrossDimension;
            fillRandom = true;
            this.fillValueMin = fillValueMin;
            this.fillValueMax = fillValueMax;
            this.consoleController = consoleController;
        }

        public override async Task<IChromosome[]> CrossOverAsync(IChromosome[] chromosomes,CancellationToken token)
        {
            var returnArray = new IChromosome[chromosomes.Length * chromosomes.Length];
            for (var i = 0; i < chromosomes.Length; i++)
            {
                for (var j = 0; j < chromosomes.Length; j++)
                {
                    
                    returnArray[i * chromosomes.Length + j] = GetCrossOver(new[] { chromosomes[i],chromosomes[j]});
                    
                }
                await Task.Delay(5,token);
            }

            return returnArray;
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
            for (var i = 0; i < c1.GetDimensionSize(acrossDimension)/ 2; i++)
            {
                pos[acrossDimension] = i;
                var val = c1.GetValuesAndPositions(pos);
                newChromosome.InsertValues(val);
            }

            var dif = c2.GetDimensionSize(acrossDimension) / 2 - c1.GetDimensionSize(acrossDimension) / 2;
            for (var i = c2.GetDimensionSize(acrossDimension) / 2; i < c2.GetDimensionSize(acrossDimension); i++)
            {
                pos[acrossDimension] = i;
                /*var values = c2.GetValuesAndPositions(pos);
                foreach (var VARIABLE in values)
                {
                    VARIABLE.Key[acrossDimension] -= dif;
                }*/
                var val = c2.GetValuesAndPositions(pos);
                
                newChromosome.InsertValues(val);
            }

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