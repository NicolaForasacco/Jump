using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilMovement : MonoBehaviour {

	public CameraController cameraController;

	public float speed = 10;

	Rigidbody2D rb;

	void Start(){
		
		rb = GetComponent<Rigidbody2D> ();
		cameraController = GameObject.FindObjectOfType<CameraController> ();

	}

	void Update () {

		transform.position -= Vector3.right * speed * Time.deltaTime;

		if (cameraController.getMinPosition () >= transform.position.y || cameraController.getXStart() >= transform.position.x) {
			Destroy(this.gameObject);
		}
	}

	public void enableGravity(){

		StartCoroutine (enableTileGravity(0.2f));
	}

	private IEnumerator enableTileGravity(float time){

		yield return new WaitForSeconds(time);

		speed = 0;
		rb.bodyType = RigidbodyType2D.Dynamic;
		rb.mass = 60;
		rb.gravityScale = 20;
	}
}
