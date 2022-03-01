using System;
using UnityEngine;

namespace Genetics.Mergers
{
    [Serializable]
    public class SimpleCut : Merger
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

        public override Chromosome Merge(Chromosome[] chromosomes)
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
                newChromosome.InsertValues(c1.GetValuesAndPositions(pos));
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
/*
            var rowDimension = acrossDimension - 1;
            if (rowDimension < 0)
            {
                rowDimension = acrossDimension + 1;
            }

            for (var i = 0; i < c1.dimensionSize[acrossDimension]/2; i++)
            {
                if ( i != rowDimension)
                {
                    for (var j = 0; j < c1.numDimensions; j++)
                    {
                        if (j != i && j != rowDimension)
                        {
                            //LoadAndApplyRow(i, j, newChromosome);
                        }
                        
                    }
                }

            }
            //var newDimensionSizes = new int[c1.numDimensions, 2];
            //var newDimensionSizes = new int[c1.numDimensions];
            
            for (var i = 0; i < c1.numDimensions; i++)
            {
                newSubDimensionSizes[i, 0] = Mathf.CeilToInt(c1.dimensionSize[i] / 2f);
                newSubDimensionSizes[i, 1] = Mathf.FloorToInt(c2.dimensionSize[i] / 2f);
                newGeneArraySize += newSubDimensionSizes[i, 0];
                newGeneArraySize += newSubDimensionSizes[i, 1];
                newDimensionSizes[i] = newSubDimensionSizes[i, 0];
                newDimensionSizes[i] += newSubDimensionSizes[i, 1];
            }

            var newGeneArray = new float[Mathf.CeilToInt(c1.totalSize / 2f) + Mathf.FloorToInt(c2.totalSize / 2f)];
            var writePoint = 0;
            for (var i = 0; i < c1.numDimensions; i++)
            {
                var d1 = c1.getValues(i);
                var d2 = c2.getValues(i);
                Array.Copy(d1,0,newGeneArray,writePoint,newSubDimensionSizes[i,0]);
                writePoint += newSubDimensionSizes[i, 0];
                Array.Copy(d2,Mathf.CeilToInt( c2.dimensionSize[i] / 2f),newGeneArray,writePoint,newSubDimensionSizes[i,1]);
                writePoint += newSubDimensionSizes[i, 1];
            }
            Array.Copy(c1.geneArray, 0, newGeneArray, writePoint, Mathf.CeilToInt(c1.totalSize / 2f));
            writePoint += Mathf.CeilToInt(c1.totalSize / 2f);
            Array.Copy(c2.geneArray, Mathf.CeilToInt(c2.totalSize / 2f), newGeneArray, writePoint,
                Mathf.FloorToInt(c2.totalSize / 2f));
            //writePoint += newSubDimensionSizes[i, 1];

            //var newChromosome = new Chromosome(newDimensionSizes);
            newChromosome.Fill(newGeneArray);*/
            return newChromosome;
        }

        private void LoadAndApplyRow(int dimension1, int dimension2,Chromosome oldChromosome, Chromosome newChromosome)
        {
            //for (var i=0;i<oldChromosome.dimensionSize[dimension1])
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
