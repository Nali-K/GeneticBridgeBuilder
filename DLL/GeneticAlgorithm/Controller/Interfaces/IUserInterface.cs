using System.Collections.Generic;

namespace GeneticAlgorithm.Controller
{
    public interface IUserInterface
    {
        void UpdateProgress(float progress);
        void UpdateActivity(Activity stage,bool isDone = false);

    }
}