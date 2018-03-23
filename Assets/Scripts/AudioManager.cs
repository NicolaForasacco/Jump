using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public AudioClip[] clips;

	public AudioSource soundtrackSource, soundEffectSource;

	public AudioClip donutPickupEffect, gameOverEffect;

	bool soundOn = true;

	int currentClip = -1;

	PreferenceManager prefM;

	void Start () {

		prefM = GameObject.FindObjectOfType<PreferenceManager> ();
 
		soundOn = prefM.getSoundOn ();

		changeBackgroundTrack ();
	}

	public void changeBackgroundTrack(){

		if (soundOn && clips.Length > 0) {
			
			currentClip++;
			if (currentClip == clips.Length)
				currentClip = 0;

			soundtrackSource.clip = clips[Random.Range(0, clips.Length - 1)];
			soundtrackSource.Play ();
		}
	}
		
	public void playDonutsPickupSound(){
		
		if (soundOn) {
			soundEffectSource.clip = donutPickupEffect;
			soundEffectSource.Play ();
		}
	}

	public void playGameOverSound(){

		if (soundOn) {
			
			soundtrackSource.volume = 0.5f;
			soundEffectSource.clip = gameOverEffect;
			soundEffectSource.Play ();
		}
	}

	public void SetSoundOn(){
		
		prefM.setSoundOn(true);
		soundOn = true;
	}

	public void SetSoundOff(){
		
		prefM.setSoundOn(false);
		soundOn = false;
	}

	public bool getSoundState(){
		return soundOn;
	}
}
