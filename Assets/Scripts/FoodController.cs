using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour {
	
	private CameraController cameraController;
	public GameObject blink;
	public objectType type;

	public enum objectType{ donut, pizza, hamburger};


	// Use this for initialization
	void Start () {
		cameraController = GameObject.FindObjectOfType<CameraController> ();
		transform.SetParent (GameObject.FindGameObjectWithTag ("foreground").transform);
	}
	// Update is called once per frame
	void Update () {
		
		if (cameraController.getMinPosition () >= transform.position.y)
			Destroy(this.gameObject);
		
	}

	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.CompareTag ("player")) {
			PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement> ();

			switch(type){
			case objectType.donut:
				pm.increaseDonutsPicked ();
				break;

			case objectType.pizza:
				//pm.increasePizzaPicked ();
				break;

			case objectType.hamburger:
				//pm.increaseHamburgerPicked ();
				break;
			}

			Instantiate (blink, transform.position, Quaternion.identity);

			Destroy (this.gameObject);
		}
	}
}
