using System.Threading;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Controller
{
    public interface IMutationFunction
    {
        string ToJson();
        bool FromJson(string json);
        Task<Chromosome> Mutate(Chromosome chromosome, CancellationToken token);
    }
}