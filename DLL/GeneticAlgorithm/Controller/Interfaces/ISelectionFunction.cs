using System.Threading;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Controller
{
    public interface ISelectionFunction
    {
        bool GetExclusive();
        Task<bool> SelectChromosome(ChromosomeScores scores, CancellationToken token);
        string ToJson();
        bool FromJson(string json);
        bool GetUnique();
        
    }
}