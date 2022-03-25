using System.Threading.Tasks;

namespace GeneticAlgorithm.Controller
{
    public interface IScores
    {
        Task<float> GetScore(int numChromosome, IFitnessFunction fitnessFunction);
    }
}