using UnityEngine;

namespace Simulation
{
    public abstract class Simulation:MonoBehaviour
    {
        public abstract void Simulate(Chromosome c,Vector3 space);
        public abstract void EndSimulation();
    }
}