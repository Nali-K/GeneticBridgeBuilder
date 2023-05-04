using System.Collections.Generic;

namespace GeneticAlgorithm.Controller.Models
{
    public class EvolutionWorld
    {
        public List<Generation> generations;
        public ChromosomeBaseData ChromosomeBaseData;
        public long AtChromosomeID => BackhandAtChromosomeID++;
        public long BackhandAtChromosomeID=0;
        private Dictionary<long,Chromosome> chromosomes= new Dictionary<long, Chromosome>();

        public EvolutionWorld()
        {
            
        }
        public EvolutionWorld(ChromosomeBaseData chromosomeBaseData)
        {

            generations = new List<Generation>();
            this.ChromosomeBaseData = chromosomeBaseData;
        }
        

        public Generation GetCurrentGeneration()
        {
            return generations[generations.Count - 1];
        }

        public int NumberFinishedGenerations()
        {
            if (GetCurrentGeneration().completed)
            {
                return generations.Count;
            }
            else
            {
                return generations.Count - 1;
            }
        }

        public Chromosome GetChromosomeByID(long id)
        {
            if (chromosomes.ContainsKey(id))
            {
                return chromosomes[id];
            }
            else
            {
                foreach (var generation in generations)
                {
                    foreach (var ch in generation.population)
                    {
                        if (!chromosomes.ContainsKey(ch.ID))
                        {
                            chromosomes.Add(ch.ID, ch);
                        }
                    }

                    foreach (var ch in generation.breedingPopulation)
                    {
                        if (!chromosomes.ContainsKey(ch.ID))
                        {
                            chromosomes.Add(ch.ID,ch);
                        }
                    }
                }
                
            }
            if (chromosomes.ContainsKey(id))
            {
                return chromosomes[id];
            }

            return null;
        }

    }
}