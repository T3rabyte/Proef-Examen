using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Android;
using System;
//sing static System.Net.Mime.MediaTypeNames;


public class GetGPSLocation : MonoBehaviour
{
    public TextMeshProUGUI GPSStatus;
    public TextMeshProUGUI latitudeValue;
    public TextMeshProUGUI longitudeValue;
    public TextMeshProUGUI altitudeValue;
    public TextMeshProUGUI horizontalAccuracyValue;
    public TextMeshProUGUI timestampValue;

    public TextMeshProUGUI accZ;
    public TextMeshProUGUI accX;

    public double yLoc;
    public double xLoc;

    public double oldXLoc;
    public double oldYLoc;

    public float oldRot;
    public float newRot;


    public GameObject player;

    public void Start() 
    {
        accX.text = GetDistance(52.39057522134929, 4.85622032093363, 52.3893487740423, 4.856272891620159).ToString();
        oldYLoc = 52.39153;
        oldXLoc = 4.857654;
        oldRot = 0;
        Input.compass.enabled = true;
        InvokeRepeating("UpdateFacing", 0f, 0.1f);
        StartCoroutine(GPSLocate());
    }

    private void Update()
    {
        UpdateLoc();
    }

    public void UpdateFacing()
    {
        newRot = (float)Math.Round((Decimal)Input.compass.trueHeading);
        if ((oldRot - newRot) > 3 || (oldRot - newRot) < -3)
        {
            player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, -(float)Math.Round((Decimal)Input.compass.trueHeading, 0, MidpointRounding.AwayFromZero));
            oldRot = (float)Math.Round((Decimal)Input.compass.trueHeading);
        }
        accX.text = Math.Round((Decimal)Input.compass.trueHeading, 0, MidpointRounding.AwayFromZero).ToString();
    }

    public void UpdateLoc() 
    {
        if (yLoc > oldYLoc) 
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(player.transform.position.x, player.transform.position.y + GetDistance(yLoc, oldXLoc, oldYLoc, oldXLoc), -2), Time.deltaTime * 8f);
            oldYLoc = yLoc;
        }
        if (yLoc < oldYLoc) 
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(player.transform.position.x, player.transform.position.y - GetDistance(yLoc, oldXLoc, oldYLoc, oldXLoc), -2), Time.deltaTime * 8f);
            oldYLoc = yLoc;
        }
        if (xLoc > oldXLoc) 
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(player.transform.position.x + GetDistance(oldYLoc, xLoc, oldYLoc, oldXLoc), player.transform.position.y, -2), Time.deltaTime * 8f);
            oldXLoc = xLoc;
        }
        if (xLoc < oldXLoc)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(player.transform.position.x - GetDistance(oldYLoc, xLoc, oldYLoc, oldXLoc), player.transform.position.y, -2), Time.deltaTime * 8f);
            oldXLoc = xLoc;
        }
        
    }

    IEnumerator GPSLocate()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }

        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
            yield break;

        // Starts the location service.
        if(Input.location.status != LocationServiceStatus.Running)
            Input.location.Start(1f,0.2f);

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            GPSStatus.text = "Timed out";
            yield break;
        }
        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            GPSStatus.text = "Unable to determine device location";
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            GPSStatus.text = "Running";
            InvokeRepeating("UpdateGPSData", 0f, 1f);
        }
    }

    private void UpdateGPSData()
    {
        Debug.Log("update");
        if (Input.location.status == LocationServiceStatus.Running)
        {
            GPSStatus.text = "Running";
            latitudeValue.text = Input.location.lastData.latitude.ToString();
            longitudeValue.text = Input.location.lastData.longitude.ToString();
            yLoc = Input.location.lastData.latitude;
            xLoc = Input.location.lastData.longitude;
            //altitudeValue.text = Input.location.lastData.altitude.ToString();
            horizontalAccuracyValue.text = Input.location.lastData.horizontalAccuracy.ToString();
            //timestampValue.text = Input.location.lastData.timestamp.ToString();
        }
        else
        {
            //Service Stopped
            GPSStatus.text = "Stop";
            Input.location.Stop();
        }
    }

    public float GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
    {
        var d1 = latitude * (Math.PI / 180.0);
        var num1 = longitude * (Math.PI / 180.0);
        var d2 = otherLatitude * (Math.PI / 180.0);
        var num2 = otherLongitude * (Math.PI / 180.0) - num1;
        var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

        return (float)(6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));
    }
}