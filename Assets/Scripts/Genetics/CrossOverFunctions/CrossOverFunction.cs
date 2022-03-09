using System;

namespace Genetics.CrossOverFunctions
{
    [Serializable]public abstract class CrossOverFunction
    {
        public abstract Chromosome CrossOver(Chromosome[] chromosomes);

    }
}