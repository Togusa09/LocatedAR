using OpenTK;
using System;
using UnityEngine;

public class GPSTrackedObject : MonoBehaviour {
    public Vector3 GpsPosition;
    public GPSManager GPSManager;

    private float degreesLatitudeInMeters = 111132;
    private float degreesLongitudeInMetersAtEquator = 111319.9f;

    // Use this for initialization
    void Start () {
		
	}

    private double GetLongitudeDegreeDistance(double latitude)
    {
        return degreesLongitudeInMetersAtEquator * Math.Cos(latitude * (Math.PI / 180));
    }

    // Update is called once per frame
    void Update()
    {
        if (GPSManager.ServiceStatus == LocationServiceStatus.Running)
        {
            var gpsPosition = new Vector3d(GPSManager.position);
            var objectPosition = new Vector3d(GpsPosition);
            var gpsAlt = 0;

            var offset = (objectPosition - gpsPosition) * new Vector3d(degreesLatitudeInMeters, 1, GetLongitudeDegreeDistance(gpsPosition.X));

            var heading = MathHelper.DegreesToRadians(GPSManager.heading);
            // Need to rotate the the the offset to align to the world coords

            var t = Quaterniond.FromEulerAngles(0, heading, 0);
            var rotatedOffset = t * offset;

            var floatPosition = new Vector3((float)rotatedOffset.X, (float)rotatedOffset.Y, (float)rotatedOffset.Z);
            transform.localPosition = floatPosition;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
