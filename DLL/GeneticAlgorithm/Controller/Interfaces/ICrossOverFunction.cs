using System.Threading;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Controller
{
    public interface ICrossOverFunction
    {

        Task<Chromosome> CrossOver(Chromosome[] chromosomes,CancellationToken token);
        
        string ToJson();
        bool FromJson(string json);
    }
}