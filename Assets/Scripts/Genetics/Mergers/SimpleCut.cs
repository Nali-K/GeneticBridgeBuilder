using System;
using UnityEngine;

namespace Genetics.Mergers
{
    [Serializable]public class SimpleCut:Merger
    {
        public SimpleCut()
        {
            
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
            var newGeneArraySize=0;
            var newSubDimensionSizes =new int[c1.numDimensions, 2];
            var newDimensionSizes =new int[c1.numDimensions];
            for (var i = 0; i < c1.numDimensions; i++)
            {
                newSubDimensionSizes[i,0] = Mathf.CeilToInt( c1.dimensionSize[i] / 2f);
                newSubDimensionSizes[i,1] = Mathf.FloorToInt( c2.dimensionSize[i] / 2f);
                newGeneArraySize += newSubDimensionSizes[i, 0];
                newGeneArraySize += newSubDimensionSizes[i, 1];
                newDimensionSizes[i] = newSubDimensionSizes[i, 0];
                newDimensionSizes[i] += newSubDimensionSizes[i, 1];
            }

            var newGeneArray = new float[newGeneArraySize];
            var writePoint = 0;
            for (var i = 0; i < c1.numDimensions; i++)
            {
                var d1 = c1.getDimension(i);
                var d2 = c2.getDimension(i);
                Array.Copy(d1,0,newGeneArray,writePoint,newSubDimensionSizes[i,0]);
                writePoint += newSubDimensionSizes[i, 0];
                Array.Copy(d2,Mathf.CeilToInt( c2.dimensionSize[i] / 2f),newGeneArray,writePoint,newSubDimensionSizes[i,1]);
                writePoint += newSubDimensionSizes[i, 1];
            }

            var newChromosome = new Chromosome(newDimensionSizes);
            newChromosome.Fill(newGeneArray);
            return newChromosome;
        }
    }
}    
