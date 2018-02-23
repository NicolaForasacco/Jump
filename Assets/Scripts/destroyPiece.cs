using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyPiece : MonoBehaviour {

	GameObject player;
	private float verticalForce = 0, horizontalForce = 0;
	public int forcePower = 12000;

	public int maxRotatingSpeed = 100;
	float rotatingSpeed;

	Camera cameraController;

	//public float hideColliderAfter = 0.2f;

	void Start () {
		
		player = GameObject.FindGameObjectWithTag ("player");

		if (player == null) {
			cameraController = GameObject.FindObjectOfType<Camera> ();
		}


		verticalForce = Random.Range (-1f, 1f);
		horizontalForce = Random.Range (-1f, 1f);

		rotatingSpeed = Random.Range (-maxRotatingSpeed, maxRotatingSpeed);

		if(GetComponent<Rigidbody2D>() != null)
			GetComponent<Rigidbody2D> ().AddForce(new Vector2(horizontalForce * forcePower, verticalForce * forcePower), ForceMode2D.Impulse);

		//hideCollider (hideColliderAfter);
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

	private void hideCollider(float time){
		StartCoroutine (hideColliderCoroutine(time));
	}

	private IEnumerator hideColliderCoroutine(float time){

		yield return new WaitForSeconds(time);

		if(GetComponent<BoxCollider2D>() != null)
			GetComponent<BoxCollider2D> ().enabled = false;

	}
}
