using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilMovement : MonoBehaviour {

	public CameraController camera;

	public float speed = 10;

	void Start(){

		camera = GameObject.FindObjectOfType<CameraController> ();

	}
	// Update is called once per frame
	void Update () {

		transform.position -= Vector3.right * speed * Time.deltaTime;

		if (camera.getXStart () >= transform.position.x) {
			Destroy(this.gameObject);
		}
	}
}
