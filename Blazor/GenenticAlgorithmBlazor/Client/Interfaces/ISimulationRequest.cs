using System.Threading.Tasks;
using GenenticAlgorithmBlazor.Shared;

namespace GenenticAlgorithmBlazor.Client.Interfaces
{
    public interface ISimulationRequest
    {
        public Task<SimulationAssigment> RequestSimulation(string page, SimulationAssigment assigment);
    }
}