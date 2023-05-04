using System;
using System.Collections.Generic;
using CommandLine.Text;
using GeneticAlgorithm.Controller;

namespace GeneticAlgorithm.Controller.Models
{


    [Serializable]public class Chromosome
    {
        public long ID{ get; set; }
        public float[] GeneArray{ get; set; }
        public int TotalSize{ get; set; }
        public List<long> Ancestors = new List<long>();

        public int NumDimensions => DimensionSize.Length;
        public int[] DimensionSize { get; set; }
        private GeneticController geneticController;
        public ChromosomeScores scores { get; set; }
        private readonly IConsoleController _consoleController;

        /// <summary>
        /// create a new list of chromosomes
        /// </summary>
        /// <param name="Amount">the amount of chromsomes</param>
        /// <param name="chromosomeShape"> the "shape of the choromosome, meaning the number of dimensions the length of each dimension"</param>
        /// <param name="fillValue">each gene will have this value</param>
        /// <param name="evolutionWorld"></param>
        /// <returns>a list of new chromosomes</returns>
        public static List<Chromosome> InitNewPopulation(int Amount, EvolutionWorld evolutionWorld,IConsoleController consoleController)
        {
            var newPopulation = new List<Chromosome>();
            var chromosomeBaseData = evolutionWorld.ChromosomeBaseData;

            for (var i = 0; i < Amount; i++)
            {
                newPopulation.Add(new Chromosome(evolutionWorld,consoleController));
                if (evolutionWorld.ChromosomeBaseData.chromosomesUseWholeNumbers)newPopulation[i].FillRandom((int)chromosomeBaseData.fillValueMin,(int)chromosomeBaseData.fillValueMax+1);
                else newPopulation[i].FillRandom(chromosomeBaseData.fillValueMin,chromosomeBaseData.fillValueMax);
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
        public Chromosome()
        {
            
        }
        public Chromosome(EvolutionWorld evolutionWorld,IConsoleController consoleController)
        {
            var chromosomeBaseData = evolutionWorld.ChromosomeBaseData;
            ID = evolutionWorld.AtChromosomeID;
            DimensionSize = new int[chromosomeBaseData.chromosomeShape.Length];
            chromosomeBaseData.chromosomeShape.CopyTo(DimensionSize,0);
            TotalSize =  chromosomeBaseData.chromosomeShape[0];
            _consoleController = consoleController;
            for (var i=1;i <chromosomeBaseData.chromosomeShape.Length;i++)
            {
                TotalSize *= chromosomeBaseData.chromosomeShape[i];

            }
            
            GeneArray = new float[TotalSize];
        }

        public static int CountSelected(List<Chromosome> c)
        {
            var count = 0;
            foreach (var chromosome in c)
            {
                if (chromosome.scores == null)
                {
                    count++;
                    continue;
                    
                }

                if (chromosome.scores.ChozenBy == null)
                {
                    count++;
                    continue;
                }
                if (chromosome.scores.ChozenBy.Count==0)
                {
                    count++;
                    continue;
                }
                count += chromosome.scores.ChozenBy.Count;
            }

            return count;
        }
        public Chromosome Copy(EvolutionWorld evolutionWorld, bool copyAncestors = false,bool copyScore =false)
        {
            var c = new Chromosome(evolutionWorld,_consoleController);
            if (copyAncestors)c.Ancestors = Ancestors;
            if (copyScore)
            {
                c.scores = new ChromosomeScores(_consoleController)
                {
                    ChozenBy = scores.ChozenBy,
                    visualisations = scores.visualisations,
                    Selected = scores.Selected,
                    Scores = scores.Scores
                };

            }
                
            
            c.Fill((float[]) GeneArray.Clone());
            return c;
        }/*
        public KeyValuePair<Chromosome,ChromosomeScores>Copy(EvolutionWorld evolutionWorld,ChromosomeScores chromosomeScores, IConsoleController consoleController,bool copyAncestors = false)
        {
            var c = new Chromosome(evolutionWorld);
            if (copyAncestors)c.Ancestors = Ancestors;
            var s = new ChromosomeScores(consoleController);
            s.visualisations = chromosomeScores.visualisations;
            s.Scores = chromosomeScores.Scores;
            s.Selected = chromosomeScores.Selected;
            s.ChozenBy = chromosomeScores.ChozenBy;
            s.Chromosome = c.ID;
            
            c.Fill((float[]) GeneArray.Clone());
            return new KeyValuePair<Chromosome, ChromosomeScores>(c,s);
        }*/
        public void Fill(float[] fillArray)
        {
            GeneArray = fillArray;
        }
        public void Fill(float value)
        {
            for (var i = 0; i < TotalSize; i++)
            {
                GeneArray[i] = value;
                
            }
        }
        public void FillRandom(int min, int max)
        {
            for (var i = 0; i < TotalSize; i++)
            {
                GeneArray[i] = RandomValueGenerator.Instance.GetRange(min, max);
            }
        }
        public void FillRandom(float min, float max)
        {
            for (var i = 0; i < TotalSize; i++)
            {
                GeneArray[i] =  RandomValueGenerator.Instance.GetRange(min, max);
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
                prevDimensionSize *= DimensionSize[i];
            }

            return GeneArray[position];

        }

        private int[] evaluatePosition(int[] position)
        {
            if (position.Length!=DimensionSize.Length)
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
                    size *= DimensionSize[i];
                    dimension = i;
                }
                else
                {
                    if (position[i] >= DimensionSize[i] || position[i] < 0)
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
            for (var i = 0; i < DimensionSize[dimension]; i++)
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
                
            var returnValue = new float[DimensionSize[dimension]];
            var prevValues=0;
            var startPosition = 0;
            var prevDimensionSize = 1;
            for (var i = 0; i < NumDimensions; i++)
            {
                startPosition += position[i] * prevDimensionSize;
                prevDimensionSize *= DimensionSize[i];
            }
            var j = startPosition;
            var jumpsize = 1;
            for (var i = 0; i < dimension; i++)
            {
                jumpsize *= DimensionSize[i];
            }
            for (var i = 0; i < DimensionSize[dimension]; i++)
            {
                returnValue[i] = GeneArray[j];
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
                prevDimensionSize *= DimensionSize[i];
            }
            var j = startPosition;
            var jumpsize = 1;
            for (var i = 0; i < dimension; i++)
            {
                jumpsize *= DimensionSize[i];
            }
            for (var i = 0; i < DimensionSize[dimension]; i++)
            {
                var writePosition = position.Clone() as int[];
                writePosition[dimension] = i;
                returnValue.Add(writePosition,GeneArray[j]);
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
            for (var i = 0; i < DimensionSize[dimension]; i++)
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
                    prevDimensionSize *= DimensionSize[i];
                }

                GeneArray[position] = VARIABLE.Value;
            }

        }

        public override String ToString()
        {
            var s = "";
            s += "Total size: ";
            s += TotalSize;
            s += "   ";
            s += DimensionSizesToString();

            s += "(Genes: [";
            s += GeneArray[0];
            for (var i = 1; i < GeneArray.Length; i++)
            {
                
                s += ", ";
                s += GeneArray[i];

                    

            }

            s += "]";
            return s;


        }

        public String DimensionSizesToString()
        {
            var s = "(Dimensions: ";
            for (var i = 0; i < DimensionSize.Length; i++)
            {
                s += "[";
                s += i;
                s += ":";
                s += DimensionSize[i];
                s += "] ";

            }

            s += ")";
            return s;
        }
    }

}