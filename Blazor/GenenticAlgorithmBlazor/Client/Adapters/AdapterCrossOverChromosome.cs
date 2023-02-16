using System.Collections.Generic;
using GeneticAlgorithm.Controller.Models;
using GeneticAlgorithm.CrossOverFunctions.Interfaces;

using ControllerChromosome = GeneticAlgorithm.Controller.Models.Chromosome;
using CrossOverChromosome = GeneticAlgorithm.CrossOverFunctions.Interfaces.IChromosome;
namespace Adapters
{
    public class AdapterCrossOverChromosome:CrossOverChromosome
    {
        public GeneticAlgorithm.Controller.Models.Chromosome chromosome;
        private EvolutionWorld _evolutionWorld;
        public List<IChromosome> Ancestors { get; set; }
        public AdapterCrossOverChromosome(ControllerChromosome chromosome,EvolutionWorld evolutionWorld)
        {
            this.chromosome = chromosome;
            this._evolutionWorld = evolutionWorld;
        }

        public IChromosome CreateNewChromosome(int[] dimensions)
        {
            return new AdapterCrossOverChromosome(ControllerChromosome.InitNewPopulation(1,_evolutionWorld)[0],_evolutionWorld);
        }

        public int GetNumDimensions()
        {
            return chromosome.NumDimensions;
        }

        public int GetDimensionSize(int dimension)
        {
            return chromosome.dimensionSize[dimension];
        }

        public void Fill(float[] values)
        {
            chromosome.Fill(values);
        }

        public void Fill(float min, float max)
        {
            chromosome.FillRandom(min,max);
        }

        public float[] GetGeneArray()
        {
            return chromosome.geneArray;
        }

        public Dictionary<int[], float> GetValuesAndPositions(int[] position)
        {
            return chromosome.GetValuesAndPositions((position));
        }

        public void InsertValues(Dictionary<int[], float> values)
        {
            chromosome.InsertValues(values);
        }

        public string ToString()
        {
            return chromosome.ToString();
        }

        
    }
}