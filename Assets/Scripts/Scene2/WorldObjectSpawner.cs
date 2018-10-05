using OpenTK;
using UnityEngine;
using UnityEngine.UI;

public class WorldObjectSpawner : MonoBehaviour
{
    public Vector3 Position;

    private bool isObjectSpawned;

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

	        var heading = MathHelper.DegreesToRadians(GPSManager.Instance.heading);
            // Need to rotate the the the offset to align to the world coords

            var t = Quaterniond.FromEulerAngles(0, heading, 0);

            var obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

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
