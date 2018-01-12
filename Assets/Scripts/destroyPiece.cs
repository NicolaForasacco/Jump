using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyPiece : MonoBehaviour {

	GameObject player;

	void Start () {
		
		player = GameObject.FindGameObjectWithTag ("player");
		hideCollider (0.4f);
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
