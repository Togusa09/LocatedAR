using System.Collections;
using UnityEngine;

public class GPSManager_NoCompass : MonoBehaviour
{
    public static GPSManager_NoCompass Instance { set; get; }
    public float latitude;
    public float longitude;

    public bool UseFakeLocation;

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
        // Allow a fake location to be returned when testing on a device that doesn't have GPS
        if (UseFakeLocation)
        {
            Debug.Log(string.Format("Using fake GPS location lat:{0} lon:{1}", latitude, longitude));
            ServiceStatus = LocationServiceStatus.Running;
            yield break;
        }

        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("user has not enabled gps");
            yield break;
        }

        // Wait for the GPS to start up so there's time to connect
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

        // If gps hasn't started by now, just give up
        ServiceStatus = Input.location.status;
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }

        //Loop forever to get GPS updates
        while (isRunning)
        {
            yield return new WaitForSeconds(2);
            UpdateGPS();
        }
    }

    private void UpdateGPS()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            ServiceStatus = Input.location.status;

            Debug.Log(string.Format("Lat: {0} Long: {1}", latitude, longitude));
        }
        else
        {
            Debug.Log("GPS is " + Input.location.status);
        }
    }
}
