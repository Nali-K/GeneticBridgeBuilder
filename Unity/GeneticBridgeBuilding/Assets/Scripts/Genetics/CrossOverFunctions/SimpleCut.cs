using System;
using UnityEngine;

namespace Genetics.CrossOverFunctions
{
    [Serializable]
    public class SimpleCut : CrossOverFunction
    {
        private int acrossDimension;
        private bool fillRandom;
        private float fillValue;
        private float fillValueMin;
        private float fillValueMax;

        public SimpleCut(int acrossDimension, float fillValue = 0f)
        {
            this.acrossDimension = acrossDimension;
            fillRandom = false;
            this.fillValue = fillValue;
        }

        public SimpleCut(int acrossDimension, float fillValueMin, float fillValueMax)
        {
            this.acrossDimension = acrossDimension;
            fillRandom = true;
            this.fillValueMin = fillValueMin;
            this.fillValueMax = fillValueMax;
        }

        public override Chromosome CrossOver(Chromosome[] chromosomes)
        {
            if (chromosomes.Length != 2)
            {
                Debug.LogError("Simple cut Merger requires precicly two chromosons");
                return null;
            }

            var c1 = chromosomes[0];
            var c2 = chromosomes[1];
            if (c1.numDimensions != c2.numDimensions)
            {
                Debug.LogError("Incompatible dimensions");
                return null;
            }

            var newGeneArraySize = calculateNewGeneArraySize(c1, c2);
            var newChromosome = new Chromosome(newGeneArraySize);
            if (fillRandom)
            {
                newChromosome.FillRandom(fillValueMin,fillValueMax);
            }
            else
            {
                var fillValues =new float[newChromosome.geneArray.Length];
                for (int i = 0; i < fillValues.Length; i++)
                {
                    fillValues[i] = fillValue;
                }
                newChromosome.Fill(fillValues);
                
            }

            var pos = new int[c1.numDimensions];
            for (var i = 0; i < c1.numDimensions; i++)
            {
                pos[i] = -1;
            }
            
            for (var i = 0; i < c1.dimensionSize[acrossDimension] / 2; i++)
            {
                pos[acrossDimension] = i;
                var val = c1.GetValuesAndPositions(pos);
                newChromosome.InsertValues(val);
            }

            var dif = c2.dimensionSize[acrossDimension] / 2 - c1.dimensionSize[acrossDimension] / 2;
            for (var i = c2.dimensionSize[acrossDimension] / 2; i < c2.dimensionSize[acrossDimension]; i++)
            {
                pos[acrossDimension] = i;
                var values = c2.GetValuesAndPositions(pos);
                foreach (var VARIABLE in values)
                {
                    VARIABLE.Key[acrossDimension] -= dif;
                }
                newChromosome.InsertValues(c2.GetValuesAndPositions(pos));
            }

            return newChromosome;
        }




        private int[] calculateNewGeneArraySize(Chromosome c1, Chromosome c2)
        {
            var newGeneArraySize = new int[c1.numDimensions];
            for (var i = 0; i < c1.numDimensions; i++)
            {
                if (i == acrossDimension)
                {
                    newGeneArraySize[i] = Mathf.CeilToInt(c1.dimensionSize[i] / 2f);
                    newGeneArraySize[i] += Mathf.FloorToInt(c2.dimensionSize[i] / 2f);
                }
                else
                {
                    if (c1.dimensionSize[i] > c2.dimensionSize[i])
                    {
                        newGeneArraySize[i] = c1.dimensionSize[i];
                    }
                    else
                    {
                        newGeneArraySize[i] =c2.dimensionSize[i];
                    }
                }
            }

            return newGeneArraySize;
        }
    }
}    
