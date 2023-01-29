using System.Threading;
using System.Threading.Tasks;
using Controllers;
using Simulation;
using UnityEngine;

namespace Views
{
    
    public class LoadAssignment:MonoBehaviour
    {
        
        [SerializeField]private SimulationController simulationController;

        public void ClickedButtonLoadAssignment()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            simulationController.HandleAssignments(cancellationTokenSource.Token);
        }
    }
}