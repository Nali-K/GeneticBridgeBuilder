namespace GeneticAlgorithm.SelectionFunctions.Interfaces
{
    public interface IFitnessFunctionData
    {
        string Name { get; }
        int NumberVoteOut { get; }
        int NumberVoteIn { get; }
        
        float MinValue { get;  }
        bool RemoveInferiorChildren { get;  }
        bool MaintainAverage { get;  }
    }
}