using System.Threading;
using System.Threading.Tasks;
using Simulation;

namespace Controllers
{
    public class LoadAssigmentFromWebApi:IAssignmentLoader
    {
        public async  Task<Assignment> LoadAssignmentAsync(CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}