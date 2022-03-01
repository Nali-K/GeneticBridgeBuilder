using UnityEngine;

namespace Simulation
{
    public abstract class Simulation
    {
        public abstract void Simulate(Chromosome c,Vector3 space);
    }
}