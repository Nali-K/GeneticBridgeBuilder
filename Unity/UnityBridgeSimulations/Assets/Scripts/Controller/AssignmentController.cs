using System.Threading;
using System.Threading.Tasks;
using Controller;
using Enums;
using Simulation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers
{
    public class AssignmentController
    {
        private Assignment currectAssignment;
        public SimulationController simulationController;

        public IAssignmentLoader assignmentLoader;
        public IAssignmentSubmitter assignmentSubmitter;
        readonly private CoroutineRunner coroutineRunner;

        private bool running = false;
        private bool stopping = false;
        private CancellationTokenSource cancellationTokenSource;
        public AssignmentController(CoroutineRunner coroutineRunner, SimulationController simulationController)
        {
            this.coroutineRunner = coroutineRunner;
            this.simulationController = simulationController;
            assignmentLoader = new LoadAssigmentFromWebApi(coroutineRunner);
            assignmentSubmitter = new SubmitAssignmentToWebApi(coroutineRunner);
            cancellationTokenSource = new CancellationTokenSource();
        }
        
        public async Task HandleAssignments(CancellationToken token)
        {
            while (!stopping)
            {
                currectAssignment = await assignmentLoader.LoadAssignmentAsync(token);
                await simulationController.RunAssignmentAsync(currectAssignment,token);
                currectAssignment.completed = true;

                var s=await assignmentSubmitter.SubmitAssignmentAsync(token, currectAssignment);
                //await AssignmentUploader.UploadAssignmentAsync(token);
                currectAssignment = null;
            }

            running = false;
        }

        public void Start()
        {
            if (!running)
            {            
                running = true;
                var token = cancellationTokenSource.Token;
                HandleAssignments(token);
            }
            
            
        }

        public void Stop(bool now = false)
        {
            stopping = true;
            Debug.Log("stop");
            if (!now) return;
            Debug.Log("NOW!");
            cancellationTokenSource.Cancel();
            running = false;

        }




    }
}