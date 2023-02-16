using System;
using System.Collections.Generic;
using GeneticAlgorithm.Controller;

namespace GeneticAlgorithm.Controller.Models
{


    [Serializable]public class Chromosome
    {
        public long ID;
        public float[] geneArray;
        public int totalSize;
        public List<Chromosome> Ancestors = new List<Chromosome>();
        public int NumDimensions => dimensionSize.Length;
        public int[] dimensionSize;
        private GeneticController geneticController;
        

        /// <summary>
        /// create a new list of chromosomes
        /// </summary>
        /// <param name="Amount">the amount of chromsomes</param>
        /// <param name="chromosomeShape"> the "shape of the choromosome, meaning the number of dimensions the length of each dimension"</param>
        /// <param name="fillValue">each gene will have this value</param>
        /// <returns>a list of new chromosomes</returns>
        public static List<Chromosome> InitNewPopulation(int Amount, EvolutionWorld evolutionWorld)
        {
            var newPopulation = new List<Chromosome>();
            var chromosomeBaseData = evolutionWorld.ChromosomeBaseData;

            for (var i = 0; i < Amount; i++)
            {
                newPopulation.Add(new Chromosome(evolutionWorld));
                if (evolutionWorld.ChromosomeBaseData.chromosomesUseWholeNumbers)newPopulation[i].FillRandom((int)chromosomeBaseData.fillValueMin,(int)chromosomeBaseData.fillValueMax);
                newPopulation[i].FillRandom(chromosomeBaseData.fillValueMin,chromosomeBaseData.fillValueMax);
            }

            return newPopulation;
        }


        /*public Chromosome(int size,GeneticController controller)
        {
            geneticController = controller;
            totalSize = size;
            dimensionSize = new int[1];
            dimensionSize[0] = size;
            geneArray = new float[size];
        }*/
        public Chromosome(EvolutionWorld evolutionWorld)
        {
            var chromosomeBaseData = evolutionWorld.ChromosomeBaseData;
            ID = evolutionWorld.AtChromosomeID;
            dimensionSize = new int[chromosomeBaseData.chromosomeShape.Length];
            chromosomeBaseData.chromosomeShape.CopyTo(dimensionSize,0);
            totalSize =  chromosomeBaseData.chromosomeShape[0];

            for (var i=1;i <chromosomeBaseData.chromosomeShape.Length;i++)
            {
                totalSize *= chromosomeBaseData.chromosomeShape[i];

            }
            
            geneArray = new float[totalSize];
        }

        public void Fill(float[] fillArray)
        {
            geneArray = fillArray;
        }
        public void Fill(float value)
        {
            for (var i = 0; i < totalSize; i++)
            {
                geneArray[i] = value;
                
            }
        }
        public void FillRandom(int min, int max)
        {
            for (var i = 0; i < totalSize; i++)
            {
                geneArray[i] = RandomValueGenerator.Instance.GetRange(min, max);
            }
        }
        public void FillRandom(float min, float max)
        {
            for (var i = 0; i < totalSize; i++)
            {
                geneArray[i] =  RandomValueGenerator.Instance.GetRange(min, max);
            }
        }

        public float GetValue(int[] atPosition)
        {
            if (atPosition.Length != NumDimensions)
            {
                geneticController.consoleController.LogError("dimension doesn't exist");
                return 0;
            }


            var position = 0;
            var prevDimensionSize = 1;
            for (var i = 0; i < NumDimensions; i++)
            {
                position += atPosition[i] * prevDimensionSize;
                prevDimensionSize *= dimensionSize[i];
            }

            return geneArray[position];

        }

        private int[] evaluatePosition(int[] position)
        {
            if (position.Length!=dimensionSize.Length)
            {
                geneticController.consoleController.LogError("invalid pos");
                return null;
                        
            }
            var numberOpenDimensions = 0;
            var dimension=0;
            var size = 1;
            for (var i = 0; i < position.Length; i++)
            {
                if (position[i] == -1)
                {
                    numberOpenDimensions += 1;
                    size *= dimensionSize[i];
                    dimension = i;
                }
                else
                {
                    if (position[i] >= dimensionSize[i] || position[i] < 0)
                    {
                        geneticController.consoleController.LogError("invalid pos");
                        return null;
                        
                    }
                }
            }

            var returnvalue = new int[3];
            returnvalue[0] = numberOpenDimensions;
            returnvalue[1] = dimension;
            returnvalue[2] = size;
            return returnvalue;
        }
        public float[] GetValues(int[] position)
        {
            var positionEvaluation = evaluatePosition(position);
            if (positionEvaluation == null)
            {
                return null;
            }
            var numberOpenDimensions = positionEvaluation[0];
            var dimension=positionEvaluation[1];
            var size = positionEvaluation[2];

            if (numberOpenDimensions == 0)
            {
                var r = new float[1];
                    r[0]= GetValue(position);
                return r;
            }

            if (numberOpenDimensions == 1)
            {
                return GetValuesRow(position, dimension, size);
            }

            var returnValues = new float[size];
            var writePoint = 0;
            for (var i = 0; i < dimensionSize[dimension]; i++)
            {
                var newpos = position.Clone() as int[];
                newpos[dimension] = i;

                var v = GetValues(newpos);
                Array.Copy(v,0,returnValues,writePoint,v.Length);
                writePoint += v.Length;
            }

            return returnValues;
        }

        public float[] GetValuesRow(int[] position, int dimension,int size)
        {
            position[dimension] = 0;
                
            var returnValue = new float[dimensionSize[dimension]];
            var prevValues=0;
            var startPosition = 0;
            var prevDimensionSize = 1;
            for (var i = 0; i < NumDimensions; i++)
            {
                startPosition += position[i] * prevDimensionSize;
                prevDimensionSize *= dimensionSize[i];
            }
            var j = startPosition;
            var jumpsize = 1;
            for (var i = 0; i < dimension; i++)
            {
                jumpsize *= dimensionSize[i];
            }
            for (var i = 0; i < dimensionSize[dimension]; i++)
            {
                returnValue[i] = geneArray[j];
                //prevValues += dimensionSize[i];
                j+=jumpsize;
            }
            //Array.Copy(geneArray,prevValues,returnValue,0,dimensionSize[dimension]);

            return (float[])returnValue;
        }
        public Dictionary<int[],float> GetValuesAndPositions(int[] pos, int dimension,int size)
        {
            var position = pos.Clone() as int[];
            position[dimension] = 0;

            var returnValue = new Dictionary<int[], float>();
            var prevValues=0;
            var startPosition = 0;
            var prevDimensionSize = 1;
            for (var i = 0; i < NumDimensions; i++)
            {
                startPosition += position[i] * prevDimensionSize;
                prevDimensionSize *= dimensionSize[i];
            }
            var j = startPosition;
            var jumpsize = 1;
            for (var i = 0; i < dimension; i++)
            {
                jumpsize *= dimensionSize[i];
            }
            for (var i = 0; i < dimensionSize[dimension]; i++)
            {
                var writePosition = position.Clone() as int[];
                writePosition[dimension] = i;
                returnValue.Add(writePosition,geneArray[j]);
                //prevValues += dimensionSize[i];
                j += jumpsize;
            }
            //Array.Copy(geneArray,prevValues,returnValue,0,dimensionSize[dimension]);

            return returnValue;
        }
        public Dictionary<int[],float> GetValuesAndPositions(int[] position)
        {
            var positionEvaluation = evaluatePosition(position);
            if (positionEvaluation == null)
            {
                return null;
            }
            var numberOpenDimensions = positionEvaluation[0];
            var dimension=positionEvaluation[1];
            var size = positionEvaluation[2];
            var returnValues = new Dictionary<int[], float>();
            if (numberOpenDimensions == 0)
            {

                returnValues.Add(position,GetValue(position));
                return returnValues;
            }

            if (numberOpenDimensions == 1)
            {
                return GetValuesAndPositions(position, dimension, size);
            }


            var writePoint = 0;
            for (var i = 0; i < dimensionSize[dimension]; i++)
            {
                var newpos = position.Clone() as int[];
                newpos[dimension] = i;

                var v = GetValuesAndPositions(newpos);
                foreach (var field in v)
                {
                    returnValues.Add(field.Key,field.Value);
                }
                writePoint += v.Count;
            }

            return returnValues;
        }
        
        public void InsertValues(Dictionary<int[],float> values)
        {

            foreach (var VARIABLE in values)
            {
                if (VARIABLE.Key.Length != NumDimensions)
                {
                    geneticController.consoleController.LogError("dimension doesn't exist");
                    return;
                }


                var position = 0;
                var prevDimensionSize = 1;
                for (var i = 0; i < NumDimensions; i++)
                {
                    position += VARIABLE.Key[i] * prevDimensionSize;
                    prevDimensionSize *= dimensionSize[i];
                }

                geneArray[position] = VARIABLE.Value;
            }

        }

        public override String ToString()
        {
            var s = "";
            s += "Total size: ";
            s += totalSize;
            s += "   ";
            s += DimensionSizesToString();

            s += "(Genes: [";
            s += geneArray[0];
            for (var i = 1; i < geneArray.Length; i++)
            {
                
                s += ", ";
                s += geneArray[i];

                    

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

}