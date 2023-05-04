using System.Collections.Generic;
using GeneticAlgorithm.Controller.Models;

namespace GeneticAlgorithm.Controller
{
    public interface IUserInterface
    {
        void UpdateProgress(float progress);
        

        void UpdateActivity(Activity stage,bool isDone = false);
        
        
        void DisplayMessage(string message);

       
    }
}