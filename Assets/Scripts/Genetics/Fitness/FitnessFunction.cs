using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class FitnessFunction
{
    public abstract Task<float> CalculateFitness(Chromosome c);

}
