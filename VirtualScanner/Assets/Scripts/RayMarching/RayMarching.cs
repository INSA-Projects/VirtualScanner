using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class RayMarching : MonoBehaviour
{
	[SerializeField]
	[Header("Render in a lower resolution to increase performance.")]
	private int downscale = 2;
	[SerializeField]
	private LayerMask volumeLayer;

	[SerializeField]
	private Shader compositeShader;
	[SerializeField]
	private Shader renderFrontDepthShader;
	[SerializeField]
	private Shader renderBackDepthShader;
	[SerializeField]
	private Shader rayMarchShader;

	[SerializeField][Header("Remove all the darker colors")]
	private bool increaseVisiblity = false;


	[Header("Drag all the textures in here")]
	[SerializeField]
	private Texture2D[] slices;
	[SerializeField][Range(0, 2)]
	private float opacity = 1;
	[Header("Volume texture size. These must be a power of 2")]
	[SerializeField]
	private int volumeWidth = 256;
	[SerializeField]
	private int volumeHeight = 256;
	[SerializeField]
	private int volumeDepth = 256;
	[Header("Clipping planes percentage")]
	[SerializeField]
	private Vector4 clipDimensions = new Vector4(100, 100, 100, 0);

	public Material _rayMarchMaterial;
	public Material _compositeMaterial;
	private Camera _ppCamera;
	public Texture3D _volumeBuffer;


    public Texture2D[] Slices
    {
        get { return this.slices; }
        set { this.slices = value; }
    }
    
	/*private void Awake()
	{
		_rayMarchMaterial = new Material(rayMarchShader);
		_compositeMaterial = new Material(compositeShader);
	}
*/
    
	private void Start()
	{	
		if (rayMarchShader == null) {
			UnityEngine.Debug.Log ("Shader null");
		}
		_rayMarchMaterial = new Material(rayMarchShader);
		_compositeMaterial = new Material(compositeShader);
		//GenerateVolumeTexture();
	}
    
	private void OnDestroy()
	{
		if(_volumeBuffer != null)
		{
			Destroy(_volumeBuffer);
		}
	}

	[SerializeField]
	private Transform clipPlane;
	[SerializeField]
	private Transform cubeTarget;
	
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		_rayMarchMaterial.SetTexture("_VolumeTex", _volumeBuffer);

		var width = source.width / downscale;
		var height = source.height / downscale;

		if(_ppCamera == null)
		{
			var go = new GameObject("PPCamera");
			_ppCamera = go.AddComponent<Camera>();
			_ppCamera.enabled = false;
		}

		_ppCamera.CopyFrom(GetComponent<Camera>());
		_ppCamera.clearFlags = CameraClearFlags.SolidColor;
		_ppCamera.backgroundColor = Color.white;
		_ppCamera.cullingMask = volumeLayer;

		var frontDepth = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGBFloat);
		var backDepth = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGBFloat);

		var volumeTarget = RenderTexture.GetTemporary(width, height, 0);

		// need to set this vector because unity bakes object that are non uniformily scaled
		//TODO:FIX
		//Shader.SetGlobalVector("_VolumeScale", cubeTarget.transform.localScale);

		// Render depths
		_ppCamera.targetTexture = frontDepth;
		_ppCamera.RenderWithShader(renderFrontDepthShader, "RenderType");
		_ppCamera.targetTexture = backDepth;
		_ppCamera.RenderWithShader(renderBackDepthShader, "RenderType");

		// Render volume
		_rayMarchMaterial.SetTexture("_FrontTex", frontDepth);
		_rayMarchMaterial.SetTexture("_BackTex", backDepth);

		if(cubeTarget != null && clipPlane != null && clipPlane.gameObject.activeSelf)
		{
			var p = new Plane(
				cubeTarget.InverseTransformDirection(clipPlane.transform.up), 
				cubeTarget.InverseTransformPoint(clipPlane.position));
			_rayMarchMaterial.SetVector("_ClipPlane", new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance));
		}
		else
		{
			_rayMarchMaterial.SetVector("_ClipPlane", Vector4.zero);
		}

		_rayMarchMaterial.SetFloat("_Opacity", opacity); // Blending strength 
		_rayMarchMaterial.SetVector("_ClipDims", clipDimensions / 100f); // Clip box


		Graphics.Blit(null, volumeTarget, _rayMarchMaterial);

		//Composite
		_compositeMaterial.SetTexture("_BlendTex", volumeTarget);
		Graphics.Blit(source, destination, _compositeMaterial);

		RenderTexture.ReleaseTemporary(volumeTarget);
		RenderTexture.ReleaseTemporary(frontDepth);
		RenderTexture.ReleaseTemporary(backDepth);
	}

	public void GenerateVolumeTexture()
	{
		// sort
		//System.Array.Sort(slices, (x, y) => x.name.CompareTo(y.name));
		
		// use a bunch of memory!
		_volumeBuffer = new Texture3D(volumeWidth, volumeHeight, volumeDepth, TextureFormat.ARGB32, false);
		
		var w = _volumeBuffer.width;
		var h = _volumeBuffer.height;
		var d = _volumeBuffer.depth;
		
		// skip some slices if we can't fit it all in
		var countOffset = (slices.Length - 1) / (float)d;
		
		var volumeColors = new Color[w * h * d];
		
		var sliceCount = 0;
		var sliceCountFloat = 0f;
		for(int z = 0; z < d; z++)
		{
			sliceCountFloat += countOffset;
			sliceCount = Mathf.FloorToInt(sliceCountFloat);
			for(int x = 0; x < w; x++)
			{
				for(int y = 0; y < h; y++)
				{
					var idx = x + (y * w) + (z * (w * h));
					volumeColors[idx] = slices[sliceCount].GetPixelBilinear(x / (float)w, y / (float)h); 
					if(increaseVisiblity)
					{
						volumeColors[idx].a *= volumeColors[idx].r;
					}
				}
			}
		}
		
		_volumeBuffer.SetPixels(volumeColors);
		_volumeBuffer.Apply();

		_rayMarchMaterial.SetTexture("_VolumeTex", _volumeBuffer);
	}

    public void setShaders(Shader composite, Shader front, Shader back, Shader ray)
    {
        compositeShader = composite;
	    renderFrontDepthShader = front;
		renderBackDepthShader = back;
		rayMarchShader = ray;
    }

    public void setTarget()
    {
        cubeTarget = GameObject.Find("Cube").GetComponent<Transform>();
		clipPlane = GameObject.Find("Clipping Plane").GetComponent<Transform>();
    }
}
