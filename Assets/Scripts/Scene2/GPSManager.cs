using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GPSManager : MonoBehaviour
{
    public static GPSManager Instance { set; get; }

    public Vector3 position;
    public Vector3 positionAccuracy;

    public float heading;
    public float headingAccuracy;

    public bool UseFakeLocation;

    public Text TextDisplay;

    private bool _isCompassPaused;
    private bool _isGPSPaused;

    [HideInInspector]
    public bool isRunning = true;

    [HideInInspector]
    public LocationServiceStatus ServiceStatus = LocationServiceStatus.Stopped;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
        ServiceStatus = LocationServiceStatus.Initializing;
        if (UseFakeLocation)
        {
            Debug.Log(string.Format("Using fake GPS location lat:{0} lon:{1} height:{2}", position.x, position.y, position.y));
            ServiceStatus = LocationServiceStatus.Running;
            yield break;
        }

        Input.compass.enabled = true;

        yield return new WaitForSeconds(5);

        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("user has not enabled gps");
            yield break;
        }

        Input.location.Start();

        yield return new WaitForSeconds(5);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0)
        {
            Debug.Log("Timed Out");
            yield break;
        }

        ServiceStatus = Input.location.status;
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to dtermine device location");
            yield break;
        }
        
        var latitude = Input.location.lastData.latitude;
        var longitude = Input.location.lastData.longitude;
        var altitude = Input.location.lastData.altitude;
        position = new Vector3(latitude, altitude, longitude);

        var hAcc = Input.location.lastData.horizontalAccuracy;
        var vAcc = Input.location.lastData.verticalAccuracy;
        positionAccuracy = new Vector3(hAcc, vAcc, hAcc);

        heading = Input.compass.trueHeading;
        headingAccuracy = Input.compass.headingAccuracy;


        while (isRunning)
        {
            UpdateGPS();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void UpdateGPS()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            if (!_isGPSPaused)
            {
                var latitude = Input.location.lastData.latitude;
                var longitude = Input.location.lastData.longitude;
                var altitude = Input.location.lastData.altitude;
                position = new Vector3(latitude, altitude, longitude);
            }
            
            if (!_isCompassPaused)
            {
                var hAcc = Input.location.lastData.horizontalAccuracy;
                var vAcc = Input.location.lastData.verticalAccuracy;
                positionAccuracy = new Vector3(hAcc, vAcc, hAcc);
                heading = Input.compass.trueHeading;
                headingAccuracy = Input.compass.headingAccuracy;
            }            

            ServiceStatus = Input.location.status;

            Debug.Log(string.Format("Lat: {0} Long: {1} Alt: {2}\nDir: {3}", position.x, position.z, position.y, heading));

            if (TextDisplay)
            {
                TextDisplay.text = string.Format("Lat: {0} Long: {1} Alt: {2}\nDir: {3}", position.x, position.z, position.y, heading);
            }
        }
        else
        {
            Debug.Log("GPS is " + Input.location.status);
        }

        
    }

    public void ToggleGPSPause(Button button)
    {
        _isGPSPaused = !_isGPSPaused;
        button.GetComponentInChildren<Text>().text = _isGPSPaused ? "Unpause GPS" : "Pause GPS";
    }

    public void ToggleCompassPause(Button button)
    {
        _isCompassPaused = !_isCompassPaused;
        button.GetComponentInChildren<Text>().text = _isCompassPaused ? "Unpause Compass" : "Pause Compass";
    }
}
