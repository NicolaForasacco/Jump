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

	void OnCollisionEnter2D(Collision2D other){

		if (other.gameObject.CompareTag ("player")) {
			Destroy (this.gameObject);
		}

		if ((other.gameObject.CompareTag("Tile") || other.gameObject.CompareTag("CloudTile") || other.gameObject.CompareTag("MovingTile")))
			transform.SetParent(other.gameObject.transform);
	}
}
