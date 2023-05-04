using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Controller.Models;

namespace GeneticAlgorithm.Controller
{
    public interface ICrossOverFunction
    {

        Task<List<Chromosome>> CrossOverAsync(List<Chromosome> chromosomes,CancellationToken token);
        
        string ToJson();
        bool FromJson(string json);
    }
}