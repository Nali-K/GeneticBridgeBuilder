using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Simulation;
using UnityEngine;
using UnityEngine.Networking;

namespace Controller
{

    public class CoroutineRunner : MonoBehaviour
    {
        public static CoroutineRunner active;
        private string json="";
        private string response="";
        private Dictionary<int, string> uploadedImages= new Dictionary<int, string>();
        private int atID = 0;
        private bool submitResult = false;
        // Start is called before the first frame update
        void Start()
        {
            active = this;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnDestroy()
        {
            active = null;
        }

        public async Task<String> RunGetBlazorChromosomeAsync(string url,CancellationToken token)
        {
            var correctResponse = false;
            do
            {
                json = "";
                //Debug.LogError("test");   
            
                var c=StartCoroutine(GetBlazorAssignment(url));

                while (json=="")
                {
                    await Task.Delay(2000,token);
                }
//                Debug.Log(json);   
                if (json == "false")
                {
//                    Debug.Log("wait");   
                    await Task.Delay(6000, token);
                }
                else
                {
                    correctResponse = true;
                }
            } while (!correctResponse && !token.IsCancellationRequested);
//            Debug.Log("dune");   

            return json;

        }
        public async Task<String> RunSubmitBlazorAssignmentAsync(string url,Assignment assignment,CancellationToken token)
        {
            submitResult = false;
            StartCoroutine(SubmitBlazorAssignment(url,assignment));
            while (!submitResult)
            {
                await Task.Delay(2000,token);
            }

            return "submitted";
        }
        public async Task<String> RunSubmitImageAsync(string url,byte[] image,CancellationToken token)
        {
            submitResult = false;
            var id = atID;
            atID++;
            StartCoroutine(SubmitImage(url,id,image));
            
            while (!uploadedImages.ContainsKey(id))
            {
                await Task.Delay(2000,token);
            }

            return uploadedImages[id];
        }
        IEnumerator GetBlazorAssignment(string url)
        {

         
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.disposeDownloadHandlerOnDispose = true;
            request.disposeUploadHandlerOnDispose = true;
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                yield break;
            
            }

            json = request.downloadHandler.text;
            request.Dispose();
        }

        IEnumerator SubmitBlazorAssignment(string url, Assignment assignment)
        {
            var wwwform = new WWWForm();
            /*wwwform.headers.Remove("Content-Type"); 
        wwwform.headers.Add("Content-Type", "application/json");*/
            wwwform.AddField("assignmentJson", JsonUtility.ToJson(assignment));
            UnityWebRequest request = UnityWebRequest.Post(url,wwwform);
            request.disposeDownloadHandlerOnDispose = true;
            request.disposeUploadHandlerOnDispose = true;
            //request.uploadHandler.contentType = "application/json";
            //request.GetResponseHeaders();
            yield return request.SendWebRequest();
        
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                yield break;
            
            }

            submitResult = true;
            request.Dispose();
        }
        IEnumerator SubmitImage(string url, int imageID, byte[] image)
        {
            var wwwform = new WWWForm();
            /*wwwform.headers.Remove("Content-Type"); 
        wwwform.headers.Add("Content-Type", "application/json");*/
            wwwform.AddField("image", Convert.ToBase64String(image));
            UnityWebRequest request = UnityWebRequest.Post(url,wwwform);
            request.disposeDownloadHandlerOnDispose = true;
            request.disposeUploadHandlerOnDispose = true;
            //request.uploadHandler.contentType = "application/json";
            //request.GetResponseHeaders();
            yield return request.SendWebRequest();
        
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                yield break;
            
            }

            uploadedImages.Add(imageID,request.downloadHandler.text);
            request.Dispose();
        }
    }
}
