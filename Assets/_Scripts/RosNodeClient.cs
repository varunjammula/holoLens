using UnityEngine;
using System.Collections;
using Vuforia;
using UnityEngine.Networking;

public class RosNodeClient : MonoBehaviour
{

    public string ApiEndpoint = "192.168.1.9";
    public GameObject arrow;
    private TextMesh messageMesh;
    private string ros_response = null;

    void Start()
    {
        ApiEndpoint = "http://" + ApiEndpoint + ":8080/api";
        Debug.Log("End point:" + ApiEndpoint);
        messageMesh = GameObject.Find("Message").GetComponent<TextMesh>();
    }

    private string getTimeStamp()
    {
        return ((long)System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
    }


    void FixedUpdate()
    {
        
        StartCoroutine(WaitForRequest(ApiEndpoint));

        
    }

    IEnumerator WaitForRequest(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Cache-Control", "max-age=0, no-cache, no-store");
        www.SetRequestHeader("Pragma", "no-cache");

        yield return www.Send();
        if (www.error == null)
        {
            Debug.Log(www.error);
            ros_response = null;
        }
        else
        {
            Debug.Log("Received " + www.downloadHandler.text);
            ros_response = www.downloadHandler.text;
            //messageMesh.text = ros_response["data"].ToString();
        }

        if (ros_response != null)
        {
            if (ros_response.Equals("1"))
            {
                //arrow.SetActive(true);
                //arrow.transform.Rotate(0, 0, 90);
                arrow.transform.rotation = Quaternion.Euler(0, 0, 90);
                messageMesh.text = "received 1";

            }
            else if (ros_response.Equals("2"))
            {
                //arrow.SetActive(true);
                //arrow.transform.Rotate(0, 0, -90);
                arrow.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else
            {
                //arrow.SetActive(false);
            }
        }
    }
}