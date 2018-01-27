using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyPiece : MonoBehaviour {

	GameObject player;
	public float verticalForce = 0, horizontalForce = 0;

	void Start () {
		
		player = GameObject.FindGameObjectWithTag ("player");

		GetComponent<Rigidbody2D> ().AddForce(new Vector2(horizontalForce * 10000, verticalForce * 10000),ForceMode2D.Impulse);

		hideCollider (0.2f);
	}

	void Update () {

		if (player.transform.position.y - transform.position.y > 200)
			GameObject.Destroy (this.gameObject);
	}

	private void hideCollider(float time){
		StartCoroutine (hideColliderCoroutine(time));
	}

	private IEnumerator hideColliderCoroutine(float time){

		yield return new WaitForSeconds(time);

		GetComponent<BoxCollider2D> ().enabled = false;
	}
}
