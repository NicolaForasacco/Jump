using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;

	private float maxShakeDuration;
	private float shakeDuration;

	private float shakeAmount;
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
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
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

	void LateUpdate()
	{
		if (active) {
			
			if (shakeDuration > 0 && playerMovement.isPlayerStill() ){

				camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
				shakeDuration -= Time.deltaTime;
				
			} else {
				active = false;
				camTransform.localPosition = originalPos;
			}
		}
	}
}
