using UnityEngine;
using System.Collections;
using MiddleVR_Unity3D;



public class RazerController : MonoBehaviour
{
    // Razer Jostick
	public vrJoystick r1;
	public vrJoystick r2;

	// Razer Node
	public GameObject tracker;


    // mouse
    public float sensitivityX = 200;
    public float sensitivityY = 200;

	public float sensitivityTranslateX = 0.05F;
	public float sensitivitiyTranslateY = 0.05F;
	public float sensitivitiyTranslateZ = 0.05F;

    public float minimumX = -8F;
    public float maximumX = 8F;
    public float minimumY = -8F;
    public float maximumY = 8F;

    float rotationY = 0F;

	public GameObject obj;

	public float oldx;
	public float oldy;


	public float deltax;
	public float deltay;

	public bool onPressX = false;
	public bool onPressY = false;
	void Start(){
		r1 = MiddleVR.VRDeviceMgr.GetJoystick (0);
		r2 = MiddleVR.VRDeviceMgr.GetJoystick (1);

		obj = GameObject.Find ("Cube");


		tracker = GameObject.Find ("HandNode");

	}



    void Update()
    {
        // keyboard
    //    this.transform.position += this.transform.forward * Input.GetAxis("Vertical") * keyboardSensitivity;
    //    this.transform.position += this.transform.right * Input.GetAxis("Horizontal") * keyboardSensitivity;
        

		if(MiddleVR.VRDeviceMgr != null){


			if(onPressX){
				if(r1.IsButtonPressed(0)){
					deltax = tracker.transform.localRotation.y - oldx;
					deltax = deltax * sensitivityX;

					Debug.Log (deltax);
					if(deltax < minimumX || deltax > maximumX){
						deltax = 0;
					}

				//	obj.transform.Rotate(new Vector3(0,deltax,0) );
					obj.transform.RotateAround( obj.transform.position, Vector3.up, deltax);


					oldx = tracker.transform.localRotation.y;
				}
				else {
					onPressX = false;
				}
			}
			if(r1.IsButtonToggled(0)){
				onPressX = true;
				oldx = tracker.transform.localRotation.y;
			}



			if(onPressY){
				if(r1.IsButtonPressed(6)){
					deltay = tracker.transform.localRotation.x - oldy;
				
					deltay = deltay * sensitivityY;
					
					if(deltay < minimumY || deltay > maximumY){
						deltay = 0;
					}
					
					//obj.transform.Rotate(new Vector3(deltay,0,0) );
					obj.transform.RotateAround( obj.transform.position, Vector3.right, deltay);
					
					oldy = tracker.transform.localRotation.x;
				}
				else {
					onPressY = false;
				}
			}
			
			
			if(r1.IsButtonToggled(6)){
				onPressY = true;
				oldy = tracker.transform.localRotation.x;
			}


			float translateX = r1.GetAxisValue(0)* sensitivityTranslateX;
			float translateY = r1.GetAxisValue(1)* sensitivitiyTranslateY;
			float translateZ = r2.GetAxisValue(1)* sensitivitiyTranslateZ;


			obj.transform.Translate (new Vector3(translateX,translateZ,translateY));

		}
  	  }
}
