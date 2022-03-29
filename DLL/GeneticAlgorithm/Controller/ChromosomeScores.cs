using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Controller
{
    public class ChromosomeScores
    {
        private Dictionary<IFitnessFunction,float> scores= new Dictionary<IFitnessFunction, float>();

        public float GetScore(IFitnessFunction fitnessFunction)
        {
            return scores[fitnessFunction];
        }
        public void AddScore(IFitnessFunction fitnessFunction, float score)
        {
            scores[fitnessFunction] = score;
        }
    }
}