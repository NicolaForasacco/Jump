using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenMovement : MonoBehaviour {

	public float minHorizontalSpeed, maxHorizontalSpeed, totalSpeed = 100;

	private CameraController cameraController;

	private float horizontalSpeed, verticalSpeed, rotateSpeed = 2000;

	void Start(){
		
		cameraController = GameObject.FindObjectOfType<CameraController> ();
		horizontalSpeed = Random.Range (minHorizontalSpeed, maxHorizontalSpeed);
		verticalSpeed = totalSpeed - horizontalSpeed;

	}

	// Update is called once per frame
	void Update () {

		transform.Rotate (Vector3.forward * rotateSpeed * Time.deltaTime);

		transform.position += new Vector3 (horizontalSpeed * Time.deltaTime, Mathf.Sin(transform.position.x / horizontalSpeed) * verticalSpeed * Time.deltaTime , transform.position.z);

		if (cameraController.getMinPosition () >= transform.position.y || cameraController.getXBounds () + 200 <= transform.position.x)
			Destroy(this.gameObject);
	}
}
