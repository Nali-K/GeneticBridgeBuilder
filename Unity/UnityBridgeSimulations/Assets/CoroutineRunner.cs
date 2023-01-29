using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Simulation;
using UnityEngine;
using UnityEngine.Networking;

public class CoroutineRunner : MonoBehaviour
{
    private string json="";

    private bool submitResult = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<String> RunGetBlazorChromosomeAsync(string url)
    {

        StartCoroutine(GetBlazorAssignment(url));
        while (json=="")
        {
            await Task.Delay(2000);
        }

        return json;

    }
    public async Task<String> RunSubmitBlazorAssignmentAsync(string url,Assignment assignment)
    {
        submitResult = false;
        StartCoroutine(SubmitBlazorAssignment(url,assignment));
        while (!submitResult)
        {
            await Task.Delay(2000);
        }

        return "submitted";
    }
    IEnumerator GetBlazorAssignment(string url)
    {
        
            
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
            yield break;
            
        }

        json = request.downloadHandler.text;
    }

    IEnumerator SubmitBlazorAssignment(string url, Assignment assignment)
    {
        var wwwform = new WWWForm();
        /*wwwform.headers.Remove("Content-Type"); 
        wwwform.headers.Add("Content-Type", "application/json");*/
        wwwform.AddField("assignmentJson", JsonConvert.SerializeObject(assignment));
        UnityWebRequest request = UnityWebRequest.Post(url,wwwform);

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

    }
}
