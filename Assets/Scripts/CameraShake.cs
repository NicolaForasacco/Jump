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
		originalPos = camTransform.localPosition;
		shakeAmount = 3f;
		maxShakeDuration = 0.1f;
		shakeDuration = maxShakeDuration;
		active = true;
	}

	public void startBigShake(){
		
		originalPos = camTransform.localPosition;
		shakeAmount = 10f;
		maxShakeDuration = 0.2f;
		shakeDuration = maxShakeDuration;
		active = true;
	}

	void Update()
	{
		if (active) {
			if (shakeDuration > 0) {
				camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

				shakeDuration -= Time.deltaTime;
			} else {
				active = false;
				camTransform.localPosition = originalPos;
			}
		}
	}
}
