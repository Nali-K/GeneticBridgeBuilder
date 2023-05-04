using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller.Models;

namespace GeneticAlgorithm.Controller
{
    public interface ISelectionFunction
    {
        bool GetExclusive();
        Task<List<Chromosome>> SelectChromosomeAsync(List<Chromosome> population, CancellationToken token);

        int GetNumberExpectedWinners();
        string ToJson();
        bool FromJson(string json);
        bool GetUnique();
        
    }
}