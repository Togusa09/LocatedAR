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
	    if (!isObjectSpawned && GPSManager.Instance.ServiceStatus == LocationServiceStatus.Running)
	    {
	        var gpsPosition = new Vector3d(GPSManager.Instance.position);
            var objectPosition = new Vector3d(Position);
            
            //var gpsLat = position.x;// * degreeInMeteres;
	        //var gpsLon = position.z;// * degreeInMeteres;

	        var offset = (objectPosition - gpsPosition) * degreeInMeteres;

	        //var latOffset = (Position.x - gpsLat) * degreeInMeteres;
	        //var lonOffset = (Position.z - gpsLon) * degreeInMeteres;

            var gpsAlt = 0;//position.y;

	        var heading = GPSManager.Instance.heading;
            // Need to rotate the the the offset to align to the world coords
            


            //WorldRoot.transform.position = new Vector3(gpsLat, gpsAlt, gpsLon);
	        //WorldRoot.transform.rotation = Quaternion.AngleAxis(heading, Vector3.up);
            //var offsetObj = new GameObject();

            //offsetObj.transform.parent = WorldRoot.transform;
            //offsetObj.transform.position = new Vector3(gpsLat, 0, gpsLon);

            


            var obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            
            var floatPosition = new Vector3((float)offset.x, (float)offset.y, (float)offset.z);
	        obj.transform.localPosition = floatPosition;
            obj.transform.localScale = new Vector3(4, 4, 4);
	        //obj.transform.parent = WorldRoot.transform;

	        if (OutputText != null)
	        {
	            OutputText.text = "Object spawned";
	        }

	        isObjectSpawned = true;
	    }
	}
}
