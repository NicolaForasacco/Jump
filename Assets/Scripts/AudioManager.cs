using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public AudioClip[] clips;
	public AudioSource soundtrackSource, donutPickUpEffectSource, jumpEffectSource, explosionEffectSource, gameOverEffectSource;

	public static AudioManager instance;

	private bool soundOn = true;
	private PreferenceManager prefM;

	private int index = 0;

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
		
		soundtrackSource.volume = 0.9f;
		index = Random.Range (0, clips.Length);
	}

	public void changeBackgroundTrack(){
		
		if (soundOn && clips.Length > 0) {
			
			index++;
			if (index == clips.Length)
				index = 0;
			
			soundtrackSource.Stop ();
			soundtrackSource.clip = clips [index];
			soundtrackSource.Play ();

		} else
			soundtrackSource.Stop ();
	}

	public void restart(){
		
		soundtrackSource.volume = 0.9f;
	}

	public void playExplosionSound(){
		
		//if (soundOn)
			explosionEffectSource.Play ();
	}

	public void playJumpSound(){

		//if (soundOn)
			jumpEffectSource.Play ();
	}
		
	public void playDonutsPickupSound(){
		
		//if (soundOn)
			donutPickUpEffectSource.Play ();
		
	}

	public void playGameOverSound(){

		//if (soundOn) {
			
			soundtrackSource.volume = 0.45f;
			gameOverEffectSource.Play ();
		//}
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
