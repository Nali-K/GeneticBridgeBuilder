using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Controller;
using Newtonsoft.Json;
using Simulation;
using UnityEngine;
using UnityEngine.Networking;

namespace Controllers
{       
    public class LoadAssigmentFromWebApi:IAssignmentLoader
    {
        private readonly CoroutineRunner coroutineRunner;

        public LoadAssigmentFromWebApi(CoroutineRunner coroutineRunner)
        {
            this.coroutineRunner = coroutineRunner;
        }
        public async  Task<Assignment> LoadAssignmentAsync(CancellationToken token)
        {
            //coroutineRunner=Object.FindObjectOfType<CoroutineRunner>();
            Debug.Log("hi");
            string url = "https://localhost:7141/assignmenttest/get_free";
            var json = await coroutineRunner.RunGetBlazorChromosomeAsync(url,token);

            Debug.Log(json);
            var a = JsonConvert.DeserializeObject<Assignment>(json);
            
            return a;
        }

    }


}
