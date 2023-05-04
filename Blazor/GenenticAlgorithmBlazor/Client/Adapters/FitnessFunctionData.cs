using GeneticAlgorithm.FitnessFunctions;
using GeneticAlgorithm.SelectionFunctions.Interfaces;

namespace Adapters
{
    public class FitnessFunctionData:IFitnessFunctionData
    {
        private readonly AdapterFitnessFunction _fitnessFunction;

        public FitnessFunctionData(AdapterFitnessFunction ff, int numberVoteOut, int numberVoteIn,
            float minValue = float.NegativeInfinity, bool removeInferiorChildren = false, bool maintainAverage = false)
        {
            _fitnessFunction = ff;
            NumberVoteOut = numberVoteOut;
            NumberVoteIn = numberVoteIn;
            MinValue = minValue;
            RemoveInferiorChildren = removeInferiorChildren;
            MaintainAverage = maintainAverage;

        }
        public string Name => _fitnessFunction.Name;
        public int NumberVoteOut { get;  }
        public int NumberVoteIn { get;  }
        public float MinValue { get;  }
        public bool RemoveInferiorChildren { get; }
        public bool MaintainAverage { get; }
    }
}