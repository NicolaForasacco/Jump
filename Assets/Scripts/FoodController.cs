using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour {
	
	private CameraController cameraController;
	public GameObject blink;

	void Start () {
		cameraController = GameObject.FindObjectOfType<CameraController> ();
		transform.SetParent (GameObject.FindGameObjectWithTag ("foreground").transform);
	}

	void Update () {
		
		if (cameraController.getMinPosition () >= transform.position.y)
			Destroy(this.gameObject);
		
	}

	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.CompareTag ("player")) {
			
			PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement> ();

			pm.increaseDonutsPicked ();
			Instantiate (blink, transform.position, Quaternion.identity);
			Destroy (this.gameObject);
		}
	}
}
