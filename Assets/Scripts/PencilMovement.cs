using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilMovement : MonoBehaviour {

	public CameraController cameraController;

	public float speed = 10;

	void Start(){

		cameraController = GameObject.FindObjectOfType<CameraController> ();

	}
	// Update is called once per frame
	void Update () {

		transform.position -= Vector3.right * speed * Time.deltaTime;

		if (cameraController.getMinPosition () >= transform.position.y) {
			Destroy(this.gameObject);
		}
	}
}
