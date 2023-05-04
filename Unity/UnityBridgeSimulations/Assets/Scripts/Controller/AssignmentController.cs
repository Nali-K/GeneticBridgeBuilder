using System;
using System.Threading;
using System.Threading.Tasks;
using Controller;
using Enums;
using Simulation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class AssignmentController
    {
        private Assignment currectAssignment;
        public SimulationController simulationController;

        public IAssignmentLoader assignmentLoader;
        public IAssignmentSubmitter assignmentSubmitter;
        public CoroutineRunner coroutineRunner;

        private bool running = false;
        private bool stopping = false;
        private CancellationTokenSource cancellationTokenSource;
        private Action _reloadScene;
        private Action _exitSimulation;
        public bool SimulationSceneLoaded=false;
        public AssignmentController()
        {

            cancellationTokenSource = new CancellationTokenSource();
        }
        
        public async Task HandleAssignments(CancellationToken token)
        {
            while (!stopping)
            {
                while (!SimulationSceneLoaded)
                {
                    await Task.Delay(100);
                }
                currectAssignment = await assignmentLoader.LoadAssignmentAsync(token);
                await simulationController.RunAssignmentAsync(currectAssignment,token);
                currectAssignment.completed = true;

                var s=await assignmentSubmitter.SubmitAssignmentAsync(token, currectAssignment);
                //await AssignmentUploader.UploadAssignmentAsync(token);
                currectAssignment = null;
                await Task.Delay(100);
                _reloadScene.Invoke();
                await Task.Delay(1000);
            }

            stopping = false;
            running = false;
            _exitSimulation.Invoke();
            await Task.Delay(1000);
        }

        public void Start(Action reloadScene,Action exitSimulation)
        {
            if (!running)
            {            
                running = true;
                var token = cancellationTokenSource.Token;
                HandleAssignments(token);
            }

            _reloadScene = reloadScene;
            _exitSimulation = exitSimulation;
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


        public void SetControllers(CoroutineRunner coroutineRunner, SimulationController simulationController)
        {
            this.coroutineRunner = coroutineRunner;
            this.simulationController = simulationController;
            assignmentLoader = new LoadAssigmentFromWebApi(coroutineRunner);
            assignmentSubmitter = new SubmitAssignmentToWebApi(coroutineRunner);
        }
    }
}