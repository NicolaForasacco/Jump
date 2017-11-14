using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenMovement : MonoBehaviour {

	private CameraController cameraController;

	public float speed = 2;
	public float rotateSpeed = 1000;

	void Start(){

		cameraController = GameObject.FindObjectOfType<CameraController> ();

	}
	// Update is called once per frame
	void Update () {

		transform.Rotate (Vector3.forward * rotateSpeed * Time.deltaTime);

		transform.position -= Vector3.up * speed * Time.deltaTime;

		if (cameraController.getMinPosition () >= transform.position.y) {
			Destroy(this.gameObject);
		}
	}
}
