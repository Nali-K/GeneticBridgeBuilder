using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

[Serializable]public class Chromosome
{
    public float[] geneArray;
    public int totalSize;
    public int numDimensions;
    public int[] dimensionSize;
    

    public Chromosome(int size)
    {
        
        totalSize = size;
        dimensionSize = new int[1];
        dimensionSize[0] = size;
        geneArray = new float[size];
    }
    public Chromosome(int[] size)
    {
        dimensionSize = new int[size.Length];
        numDimensions = size.Length;
        totalSize = 0;
        for (var i=0;i <size.Length;i++)
        {
            totalSize += size[i];
            dimensionSize[i] = size[i];
        }
        
        geneArray = new float[totalSize];
    }

    public void Fill(float[] fillArray)
    {
        geneArray = fillArray;
    }
    public void FillRandom(int min, int max)
    {
        for (var i = 0; i < totalSize; i++)
        {
            geneArray[i] = UnityEngine.Random.Range(min, max);
        }
    }
    public void FillRandom(float min, float max)
    {
        for (var i = 0; i < totalSize; i++)
        {
            geneArray[i] = UnityEngine.Random.Range(min, max);
        }
    }
    public float[] getDimension(int dimension)
    {
        if (dimension >= numDimensions || dimension < 0)
        {
            Debug.LogError("dimension doesn't exist");
            return null;
        }
        var returnValue = new float[dimensionSize[dimension]];
        var prevValues=0;
        for (var i = 0; i < dimension; i++)
        {
            prevValues += dimensionSize[i];
        }
        Array.Copy(geneArray,prevValues,returnValue,0,dimensionSize[dimension]);

        return (float[])returnValue;
    }

    public String ToString()
    {
        var s = "";
        s += "Genome:   ";
        s += "Total size: ";
        s += totalSize;
        s += "   ";
        s += DimensionSizesToString();
        var NextDimensionBreak = dimensionSize[0];
        var atDimension = 1;
        s += "(Genes: [";
        for (var i = 0; i < geneArray.Length; i++)
        {

            s += geneArray[i];
            if (i +1== NextDimensionBreak)
            {
                if (atDimension < dimensionSize.Length)
                {
                    NextDimensionBreak += dimensionSize[atDimension];
                    atDimension += 1;
                    s += "][";
                }
            }
            else
            {
                s += ",";
            }
        }

        s += "]";
        return s;


    }

    public String DimensionSizesToString()
    {
        var s = "(Dimensions: ";
        for (var i = 0; i < dimensionSize.Length; i++)
        {
            s += "[";
            s += i;
            s += ":";
            s += dimensionSize[i];
            s += "] ";

        }

        s += ")";
        return s;
    }
}
