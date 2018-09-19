﻿using OpenTK;
using UnityEngine;
using UnityEngine.UI;

public class WorldObjectSpawner : MonoBehaviour
{
    public Vector3 Position;

    private bool isObjectSpawned;

    private double degreeInMeteres = 111111;

    public Text OutputText;

    public GameObject WorldRoot;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (!isObjectSpawned && GPSManager.Instance != null && GPSManager.Instance.ServiceStatus == LocationServiceStatus.Running)
	    {
	        var gpsPosition = new Vector3d(GPSManager.Instance.position);
            var objectPosition = new Vector3d(Position);
            var gpsAlt = 0;

            var offset = (objectPosition - gpsPosition) * degreeInMeteres;

	        var heading = MathHelper.DegreesToRadians(GPSManager.Instance.heading);
            // Need to rotate the the the offset to align to the world coords

            var t = Quaterniond.FromEulerAngles(0, -heading, 0);
            var rotatedOffset = t * offset;

            var obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            
            //var floatPosition = new Vector3((float)rotatedOffset.X, (float)rotatedOffset.Y, (float)rotatedOffset.Z);
	        //obj.transform.localPosition = floatPosition;
            //obj.transform.localScale = new Vector3(4, 4, 4);

            var gpsObj = obj.AddComponent<GPSTrackedObject>();
            gpsObj.GpsPosition = Position;
            gpsObj.GPSManager = GPSManager.Instance;


            if (OutputText != null)
	        {
	            OutputText.text = "Object spawned";
	        }

	        isObjectSpawned = true;
	    }
	}
}
