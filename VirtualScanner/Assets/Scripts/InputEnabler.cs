using UnityEngine;
using System.Collections;

public class InputEnabler : MonoBehaviour{
	public int KEYBOARD = 0;
	public int RAZER = 1;
	
	public KeyboardController keyboardControl;
	//public RazerControl razerControl;
	
	void Start(){
		int device = PlayerPrefs.GetInt ("Device");
		if (device == this.KEYBOARD) {
			this.keyboardControl.enabled = false;
			//this.razerControl.enabled = true;
		} else {
			this.keyboardControl.enabled = true;
			//this.razerControl.enabled = false;
		}
		
	}
	
}