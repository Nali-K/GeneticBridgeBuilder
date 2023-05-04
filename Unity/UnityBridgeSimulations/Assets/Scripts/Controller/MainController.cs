using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Controller;
using Controllers;
using Simulation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    // Start is called before the first frame update
    private AssignmentController _assignmentController;
    public CoroutineRunner _coroutineRunner;
    public SimulationController _simulationController;
    public bool SimulationSceneLoaded = false;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadStartScene();
        _assignmentController = new AssignmentController();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_assignmentController.SimulationSceneLoaded.ToString());
    }

    public void RunSimulation()
    {
        LoadScene();

        _assignmentController.Start(() => ReloadScene(), () => ReturnToStartScene()
        );
    }

    public void Stop(bool now = false)
    {
        _assignmentController.Stop(now);
    }

    private void ReturnToStartScene()
    {
       // StartCoroutine(UnloadScene());
        LoadStartScene();
    }

    private void ReloadScene()
    {

        //var c =StartCoroutine(UnloadScene());
        LoadScene();

    }
    private void OnDestroy()
    {
        Stop(true);
    }
    private IEnumerator UnloadScene()
    {
        if (SceneManager.GetActiveScene().name == "SimulationScene")
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SimulationScene");

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("StartScene");

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }





private void LoadScene()
    {

        SceneManager.LoadScene(2,LoadSceneMode.Single);


    }

    public void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        if (scene.name == "SimulationScene")
        {
            _coroutineRunner = FindObjectOfType<CoroutineRunner>();
            _simulationController = FindObjectOfType<SimulationController>();
            _assignmentController.SetControllers(_coroutineRunner, _simulationController);
            _assignmentController.SimulationSceneLoaded = true;
        }

    }

    public void OnSceneUnloaded(Scene scene)
    {                    
        _assignmentController.SimulationSceneLoaded = false;
        
    }
    private void LoadStartScene()
    {

        SceneManager.LoadScene(1,LoadSceneMode.Single);



    }
}
