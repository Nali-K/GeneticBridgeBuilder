using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Controller
{
    public interface IMutationFunction
    {
        string ToJson();
        bool FromJson(string json);
        Task<List<Chromosome>> Mutate(List<Chromosome> chromosome, CancellationToken token);
    }
}