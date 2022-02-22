using System;

namespace Genetics.Mergers
{
    [Serializable]public abstract class Merger
    {
        public abstract Chromosome Merge(Chromosome[] chromosomes);

    }
}