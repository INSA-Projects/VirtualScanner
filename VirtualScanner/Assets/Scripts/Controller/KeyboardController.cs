using UnityEngine;
using System.Collections;
using MiddleVR_Unity3D;

public class KeyboardController : MonoBehaviour
{
	//Object to rotate
	public GameObject obj;

	// Angles
	public float _x;
	public float _y;

	// MiddleVR Mouse
	vrMouse mouse;

	void Start(){
		obj = GameObject.Find ("Cube");
		mouse = MiddleVR.VRDeviceMgr.GetMouse ();
	}


    void Update()
    {
		if (MiddleVR.VRDeviceMgr != null) {

			if (mouse.IsButtonPressed(1)) 
			{
				_x = mouse.GetAxisValue (0); //* _xSpeed;
				_y = mouse.GetAxisValue (1);// * _ySpeed;
			
				obj.transform.Rotate(Vector3.up * - _x, Space.World);
				obj.transform.Rotate(Vector3.left * _y, Space.World);
			}

		}
    }
}
