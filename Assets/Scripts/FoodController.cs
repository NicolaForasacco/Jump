using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour {
	private CameraController cameraController;

	// Use this for initialization
	void Start () {
		cameraController = GameObject.FindObjectOfType<CameraController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (cameraController.getMinPosition () >= transform.position.y) {
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.CompareTag ("player")) {
			PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement> ();
			pm.increasePoint ();
			Destroy (this.gameObject);
		}
	}
}
