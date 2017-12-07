using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudDestroyer : MonoBehaviour {
	
	GameObject player;

	float speedY;

	void Start () {
		
		player = GameObject.FindGameObjectWithTag ("player");

		float randomScale = Random.Range (2,5);

		transform.localScale = (Random.value > 0.5) ? new Vector2 (randomScale, randomScale): new Vector2 (-randomScale, randomScale);

		(GetComponent<SpriteRenderer> ()).color = new Color (1f, 1f, 1f, Random.Range(0.4f, 0.8f));

		speedX = Random.Range (- 15, 15);
	}

	float speedX;

	void Update () {

		if (player != null) {
			if (player.transform.position.y - transform.position.y > 200)
				DestroyObject (this.gameObject);
			else {
				transform.position += Vector3.right * speedX * Time.deltaTime;
			}
		}
	}
}
