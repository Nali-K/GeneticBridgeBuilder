using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Simulation
{
    public abstract class Simulation:MonoBehaviour
    {
        public abstract Task Simulate(Chromosome c,Vector3 space,CancellationToken token,int assignmentID,SimulationController simulationController,int delay);
        public abstract void EndSimulation();
        public bool started = false;
        public bool done = false;
    }
}