using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;

	// How long the object should shake for.
	public float maxShakeDuration = 10f;
	private float shakeDuration = 0f;


	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;
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
		active = true;
		originalPos = camTransform.localPosition;
		shakeAmount = 2f;
		maxShakeDuration = 0.1f;
	}

	public void startBigShake(){
		active = true;
		originalPos = camTransform.localPosition;
		shakeAmount = 6f;
		maxShakeDuration = 0.3f;
	}

	void Update()
	{
		if (active) {
			if (shakeDuration > 0) {
				camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

				shakeDuration -= Time.deltaTime * decreaseFactor;
			} else {
				active = false;
				shakeDuration = maxShakeDuration;
				camTransform.localPosition = originalPos;
			}
		}
	}
}
