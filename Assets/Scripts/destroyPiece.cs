using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyPiece : MonoBehaviour {

	GameObject player;
	Camera cameraController;
	private float verticalForce = 0, horizontalForce = 0, rotatingSpeed;

	public int forcePower = 12000, maxRotatingSpeed = 100;

	void Start () {
		
		player = GameObject.FindGameObjectWithTag ("player");

		if (player == null)
			cameraController = GameObject.FindObjectOfType<Camera> ();
		
		verticalForce = Random.Range (-1f, 1f);
		horizontalForce = Random.Range (-1f, 1f);

		rotatingSpeed = Random.Range (- maxRotatingSpeed, maxRotatingSpeed);

		if(GetComponent<Rigidbody2D>() != null)
			GetComponent<Rigidbody2D> ().AddForce(new Vector2(horizontalForce * forcePower, verticalForce * forcePower), ForceMode2D.Impulse);
	}

	void Update () {
		
		this.transform.Rotate (Vector3.forward * Time.deltaTime * rotatingSpeed, Space.World);

		if (player != null) {
			if (player.transform.position.y - transform.position.y > 200)
				GameObject.Destroy (this.gameObject);
			
		}else if (cameraController != null) {
			if (cameraController.transform.position.y - transform.position.y > 200)
				GameObject.Destroy (this.gameObject);
		}
	}
}
