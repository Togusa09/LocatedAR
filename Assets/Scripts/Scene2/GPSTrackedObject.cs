using OpenTK;
using UnityEngine;

public class GPSTrackedObject : MonoBehaviour {
    public Vector3 GpsPosition;
    public GPSManager GPSManager;

    private double degreeInMeteres = 111111;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (GPSManager.ServiceStatus == LocationServiceStatus.Running)
        {
            var gpsPosition = new Vector3d(GPSManager.position);
            var objectPosition = new Vector3d(GpsPosition);
            var gpsAlt = 0;

            var offset = (objectPosition - gpsPosition) * degreeInMeteres;

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
