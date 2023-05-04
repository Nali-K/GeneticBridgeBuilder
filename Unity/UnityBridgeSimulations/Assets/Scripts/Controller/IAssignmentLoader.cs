using System.Threading;
using System.Threading.Tasks;
using Simulation;

namespace Controllers

{
    public interface IAssignmentLoader
    {
        Task<Assignment> LoadAssignmentAsync(CancellationToken token);
    }
}