using System.Threading;
using System.Threading.Tasks;
using Simulation;

namespace Controllers
{
    public interface IAssignmentSubmitter
    {
        Task<bool> SubmitAssignmentAsync(CancellationToken token,Assignment assignment);
    }
}