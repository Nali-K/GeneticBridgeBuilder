using System;
using System.Threading;
using System.Threading.Tasks;
using Controller;
using Controllers;
using Simulation;
using UnityEngine;

namespace Views
{
    
    public class LoadAssignment:MonoBehaviour
    {
        
        [SerializeField]private SimulationController simulationController;
        private AssignmentController assignmentController;
        public CoroutineRunner coroutineRunner;
        private void Start()
        {
            assignmentController = new AssignmentController(coroutineRunner,simulationController);
        }

        public void ClickedButtonLoadAssignment()
        {

            assignmentController.Start();

        }

        public void ClickedButtonStop()
        {
            assignmentController.Stop();
        }

        private void OnDestroy()
        {
            assignmentController.Stop(true);
        }
    }
}