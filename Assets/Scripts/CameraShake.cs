using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
	
	public Transform camTransform;

	private float maxShakeDuration, shakeDuration, shakeAmount;
	public bool active = false;

	Vector3 originalPos;

	PlayerMovement playerMovement;

	void Start(){
		
		shakeDuration = maxShakeDuration;
		originalPos = camTransform.localPosition;
	}

	void Awake()
	{
		if (camTransform == null)
			camTransform = GetComponent(typeof(Transform)) as Transform;
	}

	public void startShake()
	{
		if (playerMovement == null)
			playerMovement = GameObject.FindGameObjectWithTag ("player").GetComponent<PlayerMovement>();
		
		originalPos = camTransform.localPosition;
		shakeAmount = 3f;
		maxShakeDuration = 0.1f;
		shakeDuration = maxShakeDuration;
		active = true;
	}

	public void startBigShake(){

		if (playerMovement == null)
			playerMovement = GameObject.FindGameObjectWithTag ("player").GetComponent<PlayerMovement>();
		
		originalPos = camTransform.localPosition;
		shakeAmount = 10f;
		maxShakeDuration = 0.2f;
		shakeDuration = maxShakeDuration;
		active = true;
	}

	public void startSlowMotion(){

		slowTimeLeft = 0.05f;
		Time.timeScale = 0.1f;
	}

	public void startBigBigShake(){

		if (playerMovement == null)
			playerMovement = GameObject.FindGameObjectWithTag ("player").GetComponent<PlayerMovement>();

		originalPos = camTransform.localPosition;
		shakeAmount = 15f;
		maxShakeDuration = 0.3f;
		shakeDuration = maxShakeDuration;
		active = true;
	}

	float slowTimeLeft;

	void Update(){
		
		checkTimeScale ();
	}

	void checkTimeScale(){

		if (Time.timeScale != 1) {

			slowTimeLeft -= Time.deltaTime;
			if (slowTimeLeft <= 0)
				Time.timeScale = 1;
		}
	}

	void LateUpdate()
	{
		if (active) {
			
			if (shakeDuration > 0 && playerMovement.isPlayerVerticalStill() ){

				camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
				shakeDuration -= Time.deltaTime;
				
			} else {
				active = false;
				camTransform.localPosition = originalPos;
			}
		}
	}
}
