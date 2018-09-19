using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class CameraPositionTracker : MonoBehaviour {

    public Text text;
    public Camera camera;
    public ARSessionOrigin origin;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        text.text = string.Format("{0}\n{1}", camera.transform.position.ToString(), origin.transform.position);
	}
}
