using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonBehaviour : MonoBehaviour {

	GameObject player;

	float minScale = 0.5f, maxScale = 1.5f, minSpeed = 5, maxSpeed = 15, minAlpha = 0.5f, maxAlpha = 0.9f, speed;

	void Start () {

		transform.SetParent (GameObject.FindGameObjectWithTag ("background").transform);
		player = GameObject.FindGameObjectWithTag ("player");

		float randomScale = Random.Range (minScale, maxScale);

		transform.localScale = (Random.value > 0.5)? new Vector2 (randomScale, randomScale): new Vector2 (- randomScale, randomScale);

		(GetComponent<SpriteRenderer> ()).color = new Color (1f, 1f, 1f, Random.Range(minAlpha, maxAlpha));

		speed = Random.Range (minSpeed, maxSpeed);
	}

	void Update () {

		if (player != null) {
			
			if (player.transform.position.y - transform.position.y > 400)
				Destroy (this.gameObject);
			else
				transform.Translate( Vector2.up * speed * Time.deltaTime);
		}
	}
}
