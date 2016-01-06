using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour {

    Camera middleVR;
        
	// Use this for initialization
	void Start () {
        middleVR = GameObject.Find("Camera0").GetComponent<Camera>();
        middleVR.gameObject.AddComponent<RayMarching>();
        middleVR.GetComponent<RayMarching>().enabled = true;
        middleVR.GetComponent<Camera>().gameObject.AddComponent<SliceMesh>();
        middleVR.GetComponent<SliceMesh>().enabled = true;



	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
