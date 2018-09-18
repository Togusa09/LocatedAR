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
        //var lat = -27.52587f.ConvertToRadians();
        //var lon = 152.9883f.ConvertToRadians();

        //var t1 = GPSEncoder.FromDegreesToVector3(new LatLon(lat, lon, 0), 6371000);
        //var t2 = GPSEncoder.FromVector3ToDegrees(t1, 6371000);

        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
        ServiceStatus = LocationServiceStatus.Initializing;
        if (UseFakeLocation)
        {
            Debug.Log(string.Format("Using fake GPS location lat:{0} lon:{1}", latitude, longitude));
            ServiceStatus = LocationServiceStatus.Running;
            yield break;
        }

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

        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;


        while (isRunning)
        {
            yield return new WaitForSeconds(2);
            UpdateGPS();
        }
    }

    //void Update()
    //{
    //    StartCoroutine(UpdateGPS());
    //}

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
