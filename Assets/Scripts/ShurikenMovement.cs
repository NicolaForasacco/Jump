using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenMovement : MonoBehaviour {

	public float horizontalSpeed = 60, verticalSpeed = 0.8f, rotateSpeed = 1000;

	private CameraController cameraController;

	void Start(){
		
		cameraController = GameObject.FindObjectOfType<CameraController> ();

	}

	// Update is called once per frame
	void Update () {

		transform.Rotate (Vector3.forward * rotateSpeed * Time.deltaTime);

		transform.position += new Vector3 (horizontalSpeed * Time.deltaTime, Mathf.Sin(transform.position.x) * verticalSpeed, transform.position.z);

		/*
		transform.position += Vector3.right * speed * Time.deltaTime;


		if (side) {
			if (right < wide) {
				transform.position += Vector3.up * wideSpeed * Time.deltaTime;

				right++;
			} else {
				side = !side;
				right--;
			}
		}
		else {
			if (right > -wide) {
				transform.position -= Vector3.up * wideSpeed * Time.deltaTime;

				right--;
			} else {
				side = !side;
				right++;
			}
		}
*/
		if (cameraController.getMinPosition () >= transform.position.y || cameraController.getXBounds () <= transform.position.x)
			Destroy(this.gameObject);
		
	}
}
