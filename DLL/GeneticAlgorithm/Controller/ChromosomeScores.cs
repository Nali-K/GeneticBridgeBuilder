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

        public List<float> GetAllScores()
        {
            var scoreList = new List<float>();
            foreach (var score in scores)
            {
                scoreList.Add(score.Value);
                
            }

            return scoreList;
        }

        public void AddScore(IFitnessFunction fitnessFunction, float score)
        {
            scores[fitnessFunction] = score;
        }
        
    }
}