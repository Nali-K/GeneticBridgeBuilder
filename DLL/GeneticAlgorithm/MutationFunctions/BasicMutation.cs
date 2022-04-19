using System;
using System.Collections.Generic;
using GeneticAlgorithm.MutationFunctions.Interfaces;
namespace GeneticAlgorithm.MutationFunctions
{
    public class BasicMutation
    {
        public class BasicMutator:MutationFunction
        {
            private float minValue;
            private float maxValue;
            private float mutationRatio;
            private Random rand;

//            public int[] values= new int [30];
            public BasicMutator(float min, float max, float ratio)
            {
                minValue = min;
                maxValue = max;
                mutationRatio = ratio;
                rand = new Random();
            
            }

            public override List<IChromosome> Mutate(List<IChromosome> c)
            {
                var returnList = new List<IChromosome>();
                foreach (var chromosome in c)
                {
                    c.Add(GetMutated(chromosome));
                }

                return returnList;
            }
            private IChromosome GetMutated(IChromosome chromosome)
            {
                var newGenes = new float[chromosome.GetTotalSize()];
                var i = 0;
                var geneArray = chromosome.GetGeneArray();
                foreach (var gene in geneArray)
                {
                
                    var r = (float)rand.NextDouble();
                    if (r < mutationRatio)
                    {
                        var v = GetRandomValue();
                        newGenes[i]=(v);
                    
//                        values[(int) (v + 15)]+=1;

                    }
                    else
                    {
                        newGenes[i] = gene;
                    }

                    i++;
                }
                chromosome.Fill(newGenes);
                return chromosome;
            }

            private float GetRandomValue()
            {
                var r =rand.NextDouble();
                r *= maxValue - minValue+1;
                r += minValue;

                return (float)Math.Floor(r);
            }
        }
    }
}