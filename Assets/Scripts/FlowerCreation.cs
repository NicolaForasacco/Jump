using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerCreation : MonoBehaviour {
	
	GameObject player;

	void Start () {

		player = GameObject.FindGameObjectWithTag ("player");
		transform.SetParent (GameObject.FindGameObjectWithTag ("foreground").transform);

		if (Random.value > 0.5)
			transform.localScale = new Vector2 (1, 1);
		else
			transform.localScale = new Vector2 (-1, 1);
	}

	void Update () {

		if (player != null) {
		
			if (player.transform.position.y - transform.position.y > 200)
				DestroyObject (this.gameObject);
		}
	}
}
