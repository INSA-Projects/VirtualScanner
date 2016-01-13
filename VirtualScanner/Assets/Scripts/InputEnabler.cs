using UnityEngine;
using System.Collections;

public class InputEnabler : MonoBehaviour{
	
	public KeyboardController keyboardControl;
	public RazerController razerControl;
	
	void Start(){
		int device = PlayerPrefs.GetInt ("InputMethod");
		if (device == InputMethod.RAZER) {
			this.keyboardControl.enabled = false;
			this.razerControl.enabled = true;
		} else {
			this.keyboardControl.enabled = true;
			this.razerControl.enabled = false;
		}
		
	}
	
}