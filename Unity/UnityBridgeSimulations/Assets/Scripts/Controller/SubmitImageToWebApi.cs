using System.Threading;
using System.Threading.Tasks;
using Controller;

using Simulation;
using UnityEngine;

namespace Controllers
{
    public class SubmitImageToWebApi:IImageSubmitter
    {
        

        public CoroutineRunner coroutineRunner;

        public SubmitImageToWebApi(CoroutineRunner coroutineRunner)
        {
            this.coroutineRunner = coroutineRunner;
        }
        public async Task<string> SubmitImageAsync(CancellationToken token,byte[] image,string id)
        {
            //coroutineRunner=Object.FindObjectOfType<CoroutineRunner>();
            string url = "https://localhost:7141/assignmenttest/submit_Image/"+id;
            var r = await coroutineRunner.RunSubmitImageAsync(url,image,token);


            return r;
        }


    }
}