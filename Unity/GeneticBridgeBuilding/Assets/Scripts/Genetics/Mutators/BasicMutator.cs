using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Genetics.Mutators
{
    public class BasicMutator:Mutator
    {
        private float minValue;
        private float maxValue;
        private float mutationRatio;
        private Random rand;

        public int[] values= new int [30];
        public BasicMutator(float min, float max, float ratio)
        {
            minValue = min;
            maxValue = max;
            mutationRatio = ratio;
            rand = new Random();
            
        }
        public override Chromosome Mutate(Chromosome c)
        {
            var newGenes = new float[c.totalSize];
            var i = 0;
            foreach (var gene in c.geneArray)
            {
                
                var r = (float)rand.NextDouble();
                if (r < mutationRatio)
                {
                    var v = GetRandomValue();
                    newGenes[i]=(v);
                    
                    values[(int) (v + 15)]+=1;

                }
                else
                {
                    newGenes[i] = gene;
                }

                i++;
            }
            c.Fill(newGenes);
            return c;
        }

        private float GetRandomValue()
        {
            var r =rand.NextDouble();
            r *= maxValue - minValue+1;
            r += minValue;

            return Mathf.Floor((float) r);
        }
    }
}