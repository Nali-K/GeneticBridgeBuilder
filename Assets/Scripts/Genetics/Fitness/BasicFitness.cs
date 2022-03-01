using System.Threading;
using System.Threading.Tasks;

namespace Genetics
{
    public class BasicFitness:FitnessFunction
    {
        public  BasicFitness(){
            
        }

        public override async Task<float> CalculateFitness(Chromosome c,CancellationToken token)
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