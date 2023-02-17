using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebCam : MonoBehaviour
{
    WebCamTexture webCamTexture;
    public RawImage image;
    public TMP_Text text;
    Dictionary<string, double> labels;

    // Start is called before the first frame update
    void Start()
    {
        labels = new Dictionary<string, double>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MakePhotoButton()
    {
        text.text = "1";
        StartCoroutine(Photo());
    }

    public IEnumerator Photo()
    {
        webCamTexture.Pause();
        Texture2D t = new Texture2D(webCamTexture.width, webCamTexture.height);
        t.SetPixels(webCamTexture.GetPixels());
        t.Apply();
        byte[] bytes =  t.EncodeToPNG();
        string encodedText = Convert.ToBase64String(bytes);
        yield return StartCoroutine(MakePhoto(encodedText));
        webCamTexture.Stop();
        image.texture = null;
        foreach (KeyValuePair<string, double> label in labels) 
        {
            text.text = text.text + "Des: " + label.Key + " Score: " + label.Value + " <br> ";
        }
    }

    public IEnumerator MakePhoto(string content)
    {
        post post = new post();
        post.requests = new request();
        post.requests.image = new image();
        post.requests.image.content = content;
        post.requests.features = new features();
        post.requests.features.maxResults = 10;
        post.requests.features.type = "LABEL_DETECTION";
        string json = JsonUtility.ToJson(post);

        using (UnityWebRequest webRequest = UnityWebRequest.Put("https://vision.googleapis.com/v1/images:annotate?key=", json))
        {
            webRequest.method = UnityWebRequest.kHttpVerbPOST;
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            Root response = JsonUtility.FromJson<Root>(webRequest.downloadHandler.text);
            foreach (LabelAnnotation label in response.responses[0].labelAnnotations)
                labels.Add(label.description, label.score);
        }

        yield break;
    }

    public void OpenCamera() 
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        webCamTexture = new WebCamTexture(devices[0].name, 1434, 2626, 60);      // 0 = front main lens, 1 = selfie lens wide, 2 = front wide angle lens, 3 = selfie lens upside down
        webCamTexture.autoFocusPoint = new Vector2(717, 1313);
        image.texture = webCamTexture;
        webCamTexture.Play();
    }

    public void Devices()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        foreach (WebCamDevice webcam in devices)
        {
            text.text = text.text + webcam.name + " ";
        }
    }

    public void SetReso() 
    {
        webCamTexture.Stop();
        WebCamDevice[] devices = WebCamTexture.devices;
        webCamTexture = new WebCamTexture(devices[0].name, 5454, 2890, 60);
        image.texture = webCamTexture;
        webCamTexture.Play();
    }
}

[Serializable]
public class post 
{
    public request requests;
}

[Serializable]
public class request 
{
    public image image;
    public features features;
}

[Serializable]
public class image
{
    public string content;
}

[Serializable]
public class features 
{
    public int maxResults;
    public string type;
}

[System.Serializable]
public class LabelAnnotation
{
    public string mid;
    public string description;
    public double score;
    public double topicality;
}

[System.Serializable]
public class Response
{
    public List<LabelAnnotation> labelAnnotations;
}

[System.Serializable]
public class Root
{
    public List<Response> responses;
}