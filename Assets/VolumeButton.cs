using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeButton : MonoBehaviour {

	public Sprite volumeOn,volumeOff;
	bool volumeStateOn = true;

	// Use this for initialization
	void Start () {
		
		if (volumeStateOn) {
			GetComponent<Image> ().sprite = volumeOn;
			//GetComponent<Button> ().spriteState = volumeOn;
		}
	}

	public void changeVolumeState(){
		
		volumeStateOn = !volumeStateOn;

		if (volumeStateOn) {
			GetComponent<Image> ().sprite = volumeOn;
			//GetComponent<Button> ().spriteState.pressedSprite = volumeOn;
		} else {
			GetComponent<Image> ().sprite = volumeOff;
			//GetComponent<Button> ().spriteState.pressedSprite = volumeOff;
		}
	}
}
