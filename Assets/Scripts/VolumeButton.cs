using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeButton : MonoBehaviour {

	public Sprite volumeOn,volumeOff;
	bool volumeStateOn = true;

	void Start () {

		volumeStateOn = FindObjectOfType<AudioManager> ().getSoundState ();

		if (volumeStateOn)
			GetComponent<Image> ().sprite = volumeOn;
		else
			GetComponent<Image> ().sprite = volumeOff;
		
	}

	public void changeVolumeState(){
		
		volumeStateOn = !volumeStateOn;

		if (volumeStateOn) {
			
			GetComponent<Image> ().sprite = volumeOn;
			FindObjectOfType<AudioManager> ().SetSoundOn ();
		} else {
			
			GetComponent<Image> ().sprite = volumeOff;
			FindObjectOfType<AudioManager> ().SetSoundOff ();
		}
	}
}
