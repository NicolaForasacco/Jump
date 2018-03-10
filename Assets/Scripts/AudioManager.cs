using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public Sound[] sounds;

	AudioSource audioSource;

	// Use this for initialization
	void Start () {

		if (PlayerPrefs.GetInt ("SoundOn", 1) == 1)
			soundOn = true;
		else
			soundOn = false;

		foreach(Sound a in sounds){
			
			a.source = gameObject.AddComponent<AudioSource> ();
			a.source.clip = a.clip;
			
		}
	}

	bool soundOn = true;

	public void makeSound(){
		
		if (sounds.Length != 0 && soundOn) {
			
			sounds [Random.Range(0, sounds.Length - 1)].source.Play();
		}
	}

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
