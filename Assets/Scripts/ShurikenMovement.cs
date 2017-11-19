using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenMovement : MonoBehaviour {

	private CameraController cameraController;

	public float speed = 0.6f;
	public float rotateSpeed = 1000;

	public int right = 0;
	public bool side = true;

	void Start(){

		cameraController = GameObject.FindObjectOfType<CameraController> ();

	}
	// Update is called once per frame
	void Update () {

		transform.Rotate (Vector3.forward * rotateSpeed * Time.deltaTime);

		transform.position -= Vector3.up * speed * Time.deltaTime;


		if (side) {
			if (right < 30) {
				transform.position += Vector3.right * speed/2 * Time.deltaTime;

				right++;
			} else {
				side = !side;
				right--;
			}
		}
		else {
			if (right > -30) {
				transform.position -= Vector3.right * speed/2 * Time.deltaTime;

				right--;
			} else {
				side = !side;
				right++;
			}
		}

		if (cameraController.getMinPosition () >= transform.position.y) {
			Destroy(this.gameObject);
		}
	}
}
