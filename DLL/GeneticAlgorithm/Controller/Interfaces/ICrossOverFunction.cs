using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Controller
{
    public interface ICrossOverFunction
    {

        Task<List<Chromosome>> CrossOver(List<Chromosome> chromosomes,CancellationToken token);
        
        string ToJson();
        bool FromJson(string json);
    }
}