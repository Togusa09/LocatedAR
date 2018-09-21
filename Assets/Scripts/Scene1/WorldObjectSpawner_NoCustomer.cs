using System;
using UnityEngine;
using UnityEngine.UI;

public class WorldObjectSpawner_NoCompass : MonoBehaviour
{

    public float latitude;
    public float longitutde;

    private bool isObjectSpawned;

    private float degreesLatitudeInMeters = 111132;
    private float degreesLongitudeInMetersAtEquator = 111319.9f;

    public Text OutputText;

    // Use this for initialization
    void Start () {
		
	}
	
    private float GetLongitudeDegreeDistance(float latitude)
    {
        return degreesLongitudeInMetersAtEquator * Mathf.Cos(latitude * (Mathf.PI / 180));
    }

	// Update is called once per frame
	void Update () {
	    if (!isObjectSpawned && GPSManager_NoCompass.Instance.ServiceStatus == LocationServiceStatus.Running)
	    {
	        var gpsLat = GPSManager_NoCompass.Instance.latitude;
	        var gpsLon = GPSManager_NoCompass.Instance.longitude;
	        var latOffset = (latitude - gpsLat) * degreesLatitudeInMeters;
	        var lonOffset = (longitutde - gpsLon) * GetLongitudeDegreeDistance(latitude);

	        var obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            obj.transform.position = new Vector3(latOffset, 0, lonOffset);
            obj.transform.localScale = new Vector3(4, 4, 4);

	        if (OutputText != null)
	        {
	            OutputText.text = "Object spawned";
	        }

	        isObjectSpawned = true;
	    }
	}
}
