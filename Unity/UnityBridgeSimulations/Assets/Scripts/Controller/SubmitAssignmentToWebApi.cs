using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Simulation;
using UnityEngine;

namespace Controllers
{
    public class SubmitAssignmentToWebApi:IAssignmentSubmitter
    {
        

        public CoroutineRunner coroutineRunner;

        public SubmitAssignmentToWebApi(CoroutineRunner coroutineRunner)
        {
            this.coroutineRunner = coroutineRunner;
        }
        public async Task<bool> SubmitAssignmentAsync(CancellationToken token,Assignment assignment)
        {
            //coroutineRunner=Object.FindObjectOfType<CoroutineRunner>();
            string url = "https://localhost:7141/assignmenttest/submit_completed";
            var r = await coroutineRunner.RunSubmitBlazorAssignmentAsync(url,assignment);


            return r=="submitted";
        }


    }
}