using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public AudioClip[] clips;
	public AudioSource soundtrackSource, soundEffectSource;
	public AudioClip donutPickupEffect, gameOverEffect;

	public static AudioManager instance;

	private bool soundOn = true;
	private PreferenceManager prefM;

	void Awake(){
		
		prefM = GameObject.FindObjectOfType<PreferenceManager> ();
		soundOn = prefM.getSoundOn ();

		if (instance == null) {
			instance = this;
			changeBackgroundTrack ();
		}else
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	void Start () {
		soundtrackSource.volume = 1;
	}

	public void changeBackgroundTrack(){
		
		if (soundOn && clips.Length > 0) {
			
			soundtrackSource.Stop ();
			int rc = Random.Range (0, clips.Length);

			while (soundtrackSource.clip == clips [rc])
				rc = Random.Range (0, clips.Length);

			soundtrackSource.clip = clips [rc];
			soundtrackSource.Play ();

		} else
			soundtrackSource.Stop ();
	}

	public void restart(){
		
		soundtrackSource.volume = 1f;
	}
		
	public void playDonutsPickupSound(){
		
		if (soundOn) {
			soundEffectSource.clip = donutPickupEffect;
			soundEffectSource.Play ();
		}
	}

	public void playGameOverSound(){

		if (soundOn) {
			
			soundtrackSource.volume = 0.3f;
			soundEffectSource.clip = gameOverEffect;
			soundEffectSource.Play ();
		}
	}

	public void SetSoundOn(){
		
		prefM.setSoundOn(true);
		soundOn = true;

		if (soundtrackSource.clip != null)
			soundtrackSource.Play ();
		else
			changeBackgroundTrack ();
	}

	public void SetSoundOff(){
		
		prefM.setSoundOn(false);
		soundOn = false;
		soundtrackSource.Stop ();
	}

	public bool getSoundState(){
		return soundOn;
	}
}
