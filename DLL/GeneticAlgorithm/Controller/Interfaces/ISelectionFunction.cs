using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Controller
{
    public interface ISelectionFunction
    {
        bool GetExclusive();
        Task<List<Chromosome>> SelectChromosomeAsync(Dictionary<Chromosome,ChromosomeScores> scores, CancellationToken token);
        string ToJson();
        bool FromJson(string json);
        bool GetUnique();
        
    }
}