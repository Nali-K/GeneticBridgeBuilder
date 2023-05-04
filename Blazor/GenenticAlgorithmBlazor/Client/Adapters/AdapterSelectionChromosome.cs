using System;
using System.Collections.Generic;
using System.Diagnostics;
using GeneticAlgorithm.Controller;
using GeneticAlgorithm.Controller.Models;
using GeneticAlgorithm.SelectionFunctions.Interfaces;

namespace Adapters
{
    public class AdapterSelectionChromosome:GeneticAlgorithm.SelectionFunctions.Interfaces.IChromosome
    {
        ChromosomeScores scores;
        private EvolutionWorld _evolutionWorld;
        private Chromosome _chromosome;
        private List<IChromosome> _parents;
        public AdapterSelectionChromosome(Chromosome chromosome,EvolutionWorld evolutionWorld)
        {
            this.scores = chromosome.scores;
            _evolutionWorld = evolutionWorld;
            _chromosome = chromosome;

        }
        public List<float> GetScores()
        {
            var scoreList = new List<float>();
            foreach (var score in scores.Scores)
            {
                scoreList.Add(score.Value);
            }

            return scoreList;
        }

        public Dictionary<string, float> GetScoresAndFitnessFunctionNames()
        {
            return scores == null ? null : scores.Scores;
        }

        public List<string> ChozenBy { get; set; }
        public List<IChromosome> parents
        {
            get
            {
                if (_parents != null) return _parents;
                _parents = new List<IChromosome>();
                if (_chromosome.Ancestors is not {Count: > 0}) return null;
                foreach (var ancestor in _chromosome.Ancestors)
                {
                    _parents.Add(new AdapterSelectionChromosome(_evolutionWorld.GetChromosomeByID(ancestor),
                        _evolutionWorld));
                }

                return _parents; }
        }

        public Chromosome GetChromosome()
        {
            return _chromosome;
        }
    }
}