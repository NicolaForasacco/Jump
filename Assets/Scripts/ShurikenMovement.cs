using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenMovement : MonoBehaviour {

	private CameraController cameraController;

	public float horizontalSpeed = 1f;
	public float verticalSpeed = 1.8f;
	public int horizontalWide = 40;

	public float rotateSpeed = 1000;

	public int right = 0;
	public bool side = true;

	void Start(){

		cameraController = GameObject.FindObjectOfType<CameraController> ();

	}
	// Update is called once per frame
	void Update () {

		transform.Rotate (Vector3.forward * rotateSpeed * Time.deltaTime);

		transform.position += Vector3.right * verticalSpeed * Time.deltaTime;


		if (side) {
			if (right < horizontalWide) {
				transform.position += Vector3.up * horizontalSpeed * Time.deltaTime;

				right++;
			} else {
				side = !side;
				right--;
			}
		}
		else {
			if (right > -horizontalWide) {
				transform.position -= Vector3.up * horizontalSpeed * Time.deltaTime;

				right--;
			} else {
				side = !side;
				right++;
			}
		}

		if (cameraController.getMinPosition () >= transform.position.y || cameraController.getXBounds () <= transform.position.x) {
			Destroy(this.gameObject);
		}
	}
}
