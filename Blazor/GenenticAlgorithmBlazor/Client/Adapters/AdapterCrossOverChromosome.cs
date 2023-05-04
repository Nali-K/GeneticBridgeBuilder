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
        private ConsoleController _consoleController;
        public List<IChromosome> Ancestors
        {
            set => SetAncestors(value);
        }

        public long ID => chromosome.ID;
        public int ChozenBy { get; }

        public AdapterCrossOverChromosome(ControllerChromosome chromosome,EvolutionWorld evolutionWorld, int chozenBy, ConsoleController consoleController)
        {
            this.chromosome = chromosome;
            this._evolutionWorld = evolutionWorld;
            ChozenBy = chozenBy;
            _consoleController = consoleController;
        }


        private void SetAncestors(List<IChromosome> ancestors)
        {
            if (chromosome.Ancestors == null)
                chromosome.Ancestors = new List<long>();
            foreach (var c in ancestors)
            {
                chromosome.Ancestors.Add(c.ID);
            }
        }
        public IChromosome CreateNewChromosome(int[] dimensions)
        {
            return new AdapterCrossOverChromosome(ControllerChromosome.InitNewPopulation(1,_evolutionWorld,_consoleController)[0],_evolutionWorld,-1,_consoleController);
        }

        public int GetNumDimensions()
        {
            return chromosome.NumDimensions;
        }

        public int GetDimensionSize(int dimension)
        {
            return chromosome.DimensionSize[dimension];
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
            return chromosome.GeneArray;
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