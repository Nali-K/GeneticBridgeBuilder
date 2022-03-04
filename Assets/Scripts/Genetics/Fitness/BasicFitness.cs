using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Genetics
{
    [Serializable]public class BasicFitness:FitnessFunction
    {
        public  BasicFitness(){
            
        }

        public override async Task<float> CalculateFitness(Chromosome c, CancellationToken token, UnityEngine.Vector3 position)
        {
            var score = 0f;
            for (var i=0; i < c.totalSize; i++)
            {
                score += c.geneArray[i];
            }

            score /= c.totalSize;

            return score;
        }


    }
}