using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilMovement : MonoBehaviour {


	public GameObject explosion;
	private float speed;

	Rigidbody2D rb;
	CameraController cameraController;

	public float minSpeed = 50, maxSpeed = 80;

	void Start(){
		
		rb = GetComponent<Rigidbody2D> ();
		cameraController = GameObject.FindObjectOfType<CameraController> ();
		speed = Random.Range (minSpeed, maxSpeed);

	}

	void Update () {

		transform.position -= Vector3.right * speed * Time.deltaTime;

		if (cameraController.getMinPosition () >= transform.position.y || cameraController.getXStart() >= transform.position.x) {
			Destroy(this.gameObject);
		}
	}

	public void showExplosion(){

		enableGravity (0.2f);
		showExplosion (0.4f);
	}

	private void enableGravity(float waitFor){

		StartCoroutine (enableTileGravity(waitFor));
	}

	private IEnumerator enableTileGravity(float time){

		yield return new WaitForSeconds(time);

		rb.bodyType = RigidbodyType2D.Dynamic;
		rb.mass = 60;
		rb.gravityScale = 20;
	}


	private void showExplosion(float time){

		StartCoroutine (showExplosionCoroutine(time));
	}

	private IEnumerator showExplosionCoroutine(float time){

		yield return new WaitForSeconds(time);

		Instantiate(explosion, transform.position, Quaternion.identity);

		GameObject.Destroy (this.gameObject);
	}
}
