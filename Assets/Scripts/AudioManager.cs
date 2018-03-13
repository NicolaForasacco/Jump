using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	//public Sound[] sounds;
	public AudioClip[] clips;

	AudioSource audioSource;
	bool soundOn = true;
	int currentClip = -1;

	// Use this for initialization
	void Start () {

		audioSource = GetComponent<AudioSource> ();

		if (PlayerPrefs.GetInt ("SoundOn", 1) == 1)
			soundOn = true;
		else
			soundOn = false;

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

			audioSource.clip = clips[currentClip];
			audioSource.Play ();
		}
	
	}


	/*public void makeSound(){
		
		if (sounds.Length != 0 && soundOn) {
			
			sounds [Random.Range(0, sounds.Length - 1)].source.Play();
		}
	}*/

	public void SetSoundOn(){
		
		PlayerPrefs.SetInt ("SoundOn", 1);
		soundOn = true;
	}

	public void SetSoundOff(){
		
		PlayerPrefs.SetInt ("SoundOn", 0);
		soundOn = false;
	}

	public bool getSoundState(){
		return soundOn;
	}
}
