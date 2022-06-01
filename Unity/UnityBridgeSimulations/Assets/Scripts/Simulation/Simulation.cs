using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Simulation
{
    public abstract class Simulation:MonoBehaviour
    {
        public abstract Task Simulate(Chromosome c,Vector3 space,CancellationToken token);
        public abstract void EndSimulation();
    }
}