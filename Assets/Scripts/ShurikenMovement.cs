using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenMovement : MonoBehaviour {

	private CameraController cameraController;

	public float wideSpeed = 1f;
	public float speed = 1.8f;
	public int wide = 40;

	public float rotateSpeed = 1000;

	public int right = 0;
	public bool side = true;

	void Start(){
		
		cameraController = GameObject.FindObjectOfType<CameraController> ();

	}
	// Update is called once per frame
	void Update () {

		transform.Rotate (Vector3.forward * rotateSpeed * Time.deltaTime);

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

		if (cameraController.getMinPosition () >= transform.position.y || cameraController.getXBounds () <= transform.position.x) {
			Destroy(this.gameObject);
		}
	}
}
