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

	// Use this for initialization
	void Start () {

		prefM = GameObject.FindObjectOfType<PreferenceManager> ();
 
		soundOn = prefM.getSoundOn ();

		changeBackgroundTrack ();

		/*foreach(Sound a in sounds){
			
			a.source = gameObject.AddComponent<AudioSource> ();
			a.source.clip = a.clip;
			
		}*/
	}

	public void changeBackgroundTrack(){

		if (soundOn && clips.Length > 0) {
			
			currentClip++;
			if (currentClip == clips.Length)
				currentClip = 0;

			soundtrackSource.clip = clips[currentClip];
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
			soundEffectSource.clip = gameOverEffect;
			soundEffectSource.Play ();
		}
	}


	/*public void makeSound(){
		
		if (sounds.Length != 0 && soundOn) {
			
			sounds [Random.Range(0, sounds.Length - 1)].source.Play();
		}
	}*/

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
