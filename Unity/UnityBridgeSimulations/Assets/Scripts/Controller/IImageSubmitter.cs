using System.Threading;
using System.Threading.Tasks;
using Simulation;

namespace Controllers
{
    public interface IImageSubmitter
    {
        Task<string> SubmitImageAsync(CancellationToken token,byte[] image);
    }
}