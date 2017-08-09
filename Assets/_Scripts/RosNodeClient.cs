using UnityEngine;
using System.Collections;
using Vuforia;



#if !UNITY_EDITOR
using Windows.Web.Http;
using System;
#endif



public class RosNodeClient : MonoBehaviour
{

    public string ApiEndpoint = "192.168.86.102";
    private TextMesh messageMesh;
    private string ros_response = null;
    private DefaultTrackableEventHandler trackingHandler = null;

#if UNITY_EDITOR
    private WWW www;
#endif

#if !UNITY_EDITOR
    private HttpClient httpClient = null;
#endif

    void Start()
    {
        ApiEndpoint = "http://" + ApiEndpoint + ":8080/api";
        Debug.Log("End point:" + ApiEndpoint);
        messageMesh = GameObject.Find("Message").GetComponent<TextMesh>();
        trackingHandler = new DefaultTrackableEventHandler();

#if UNITY_EDITOR
        www = new WWW(ApiEndpoint);
#endif

#if !UNITY_EDITOR
        //httpClient = new HttpClient();
#endif

    }

    private string getTimeStamp()
    {
        return ((long)System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
    }


    void FixedUpdate()
    {
        messageMesh.text = "Time: " + getTimeStamp() + " ms.";
#if UNITY_EDITOR
        //StartCoroutine(WaitForRequest(www));
#endif


#if !UNITY_EDITOR
        //SendRequest();
#endif

        if (ros_response != null)
            messageMesh.text = ros_response;
    }

#if UNITY_EDITOR
    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
            ros_response = www.text;
            //messageMesh.text = Time.deltaTime.ToString();
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
#endif

#if !UNITY_EDITOR
    private async void SendRequest()
    {
        var headers = httpClient.DefaultRequestHeaders;

        //The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
        //especially if the header value is coming from user input.
        string header = "ie";
        if (!headers.UserAgent.TryParseAdd(header))
        {
            throw new Exception("Invalid header value: " + header);
        }

        header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
        if (!headers.UserAgent.TryParseAdd(header))
        {
            throw new Exception("Invalid header value: " + header);
        }

        Uri requestUri = new Uri(ApiEndpoint);

        //Send the GET request asynchronously and retrieve the response as a string.
        HttpResponseMessage httpResponse = new HttpResponseMessage();
        string httpResponseBody = "";

        try
        {
            //Send the GET request
            httpResponse = await httpClient.GetAsync(requestUri);
            httpResponse.EnsureSuccessStatusCode();
            httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            ros_response = httpResponseBody.ToString();
        }
        catch (Exception ex)
        {
            httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
        }
    }
#endif
}