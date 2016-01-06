using UnityEngine;
using System.Collections;

public class SliceMesh : MonoBehaviour
{
	public MeshFilter[] meshes;
	private Mesh[] _outputMesh;
	private Transform _planeTrans;

	private void Start()
	{
		var planeObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
		planeObj.GetComponent<Renderer>().enabled = false;

		_planeTrans = planeObj.transform;

		planeObj.transform.parent = GameObject.Find("Camera0").transform;
		_planeTrans.localPosition = new Vector3(0, 0, GameObject.Find("Camera0").GetComponent<Camera>().nearClipPlane + 0.001f);
		_planeTrans.localRotation = Quaternion.Euler(-90, 0, 0);

		_outputMesh = new Mesh[meshes.Length];
		for(int i = 0; i < _outputMesh.Length; i++)
		{
			_outputMesh[i] = new Mesh();
		}
	}
	
	private void Update()
	{
		var cuttingPlane = new Plane(_planeTrans.up, _planeTrans.position);
		for(int i = 0; i < meshes.Length; i++)
		{
			if(_outputMesh[i] == null){
				UnityEngine.Debug.Log("_outputMesh");
			}
			if(meshes[i] == null){
				UnityEngine.Debug.Log("meshes");
			}
			MeshSlicer.CutTriangleMeshOneSide(_outputMesh[i], meshes[i].mesh, cuttingPlane, meshes[i].transform, _planeTrans, false, true);
			Graphics.DrawMesh(_outputMesh[i], meshes[i].transform.localToWorldMatrix, meshes[i].GetComponent<Renderer>().material, 0);
		}
	}

	public void setSize(MeshFilter m)
	{
		meshes = new MeshFilter[1];
		UnityEngine.Debug.Log ("Start Init SliceMesh");
		meshes [0] = m;
	}
}
