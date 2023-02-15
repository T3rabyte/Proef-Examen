using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class WebCam : MonoBehaviour
{
    WebCamTexture webCamTexture;
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite)) 
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCamera() 
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }

        webCamTexture = new WebCamTexture();
        webCamTexture.Play();
        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        //Encode to a PNG
        byte[] bytes = photo.EncodeToPNG();
        //Write out the PNG. Of course you have to substitute your_path for something sensible
        File.WriteAllBytes(Application.persistentDataPath + "/" + "photo.png", bytes);
        text.text = Application.persistentDataPath;
    }
}
