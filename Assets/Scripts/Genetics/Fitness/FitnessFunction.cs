using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]public abstract class FitnessFunction
{
      public abstract Task<float> CalculateFitness(Chromosome c,CancellationToken token,Vector3 position);


}
