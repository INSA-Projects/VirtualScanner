using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour 
{
    public GameObject defaultUser;
        
	// Use this for initialization
	void Start () 
    {
        GameObject middleVRObject = GameObject.Find("Camera0");
        middleVRObject.AddComponent<RayMarching>();
        

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
