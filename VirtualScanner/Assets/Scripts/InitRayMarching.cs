using UnityEngine;
using System.Collections;

public class InitRayMarching : MonoBehaviour {


    public Shader compositeShader;

    public Shader renderFrontDepthShader;

    public Shader renderBackDepthShader;

    public Shader rayMarchShader;

	public MeshFilter mesh;


	// Use this for initialization  
	void Start () {
		UnityEngine.Debug.Log ("Start init  RayMarching");
        RayMarching ray = GameObject.Find("Camera0").GetComponent<RayMarching>();
        ray.setShaders(compositeShader,renderFrontDepthShader,renderBackDepthShader,rayMarchShader);
		ray.setTarget ();

		SliceMesh s = GameObject.Find ("Camera0").GetComponent<SliceMesh> ();
		s.setSize (mesh);
		UnityEngine.Debug.Log ("End init RayMarching");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
