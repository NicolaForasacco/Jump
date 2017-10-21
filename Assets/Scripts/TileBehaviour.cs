using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour {
	
	private GameObject player;

	void Start () {}

	void Update () {
		
		if (player != null) {
			if (player.transform.position.y - transform.position.y > 10)
				DestroyObject (this.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D collision){

		if (collision.gameObject.CompareTag ("player"))
			player = collision.gameObject;
	}
}
