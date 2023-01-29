using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class FillTextFromBlazor : MonoBehaviour
{
    public Text text;
    public Text textChromsome;
    private IEnumerator test;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPressed()
    {
        StartCoroutine(GetBlazorText());
    }
    public void ButtonChromosomePressed()
    {
        StartCoroutine(GetBlazorChromosome());
    }
    IEnumerator GetBlazorText()
    {
        
        string url = "http://localhost:5027/SimpleText";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
            yield break;
            
        }

        text.text = request.downloadHandler.text;
    }
    IEnumerator GetBlazorChromosome()
    {
        
        string url = "http://localhost:5027/chromosome";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
            yield break;
            
        }

        text.text = request.downloadHandler.text;
    }
}
