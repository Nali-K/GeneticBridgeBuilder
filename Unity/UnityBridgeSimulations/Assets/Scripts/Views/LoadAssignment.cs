using System;
using System.Threading;
using System.Threading.Tasks;
using Controller;
using Controllers;
using Simulation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Views
{
    
    public class LoadAssignment:MonoBehaviour
    {
        public MainController MainController;
        private void Start()
        {
            MainController = FindObjectOfType<MainController>();

        }

        public void ClickedButtonLoadAssignment()
        {

            MainController.RunSimulation();

        }

        public void ClickedButtonStop()
        {
            MainController.Stop();
        }


    }
}