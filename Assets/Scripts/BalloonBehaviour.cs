using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonBehaviour : MonoBehaviour {

	GameObject player;

	int speed;

	void Start () {
		transform.SetParent (GameObject.FindGameObjectWithTag ("background").transform);
		player = GameObject.FindGameObjectWithTag ("player");

		float randomScale = Random.Range (0.5f, 1.5f);

		transform.localScale = (Random.value > 0.5)? new Vector2 (randomScale, randomScale): new Vector2 (-randomScale, randomScale);

		(GetComponent<SpriteRenderer> ()).color = new Color (1f, 1f, 1f, Random.Range(0.8f, 1));

		speed = Random.Range (5, 15);
	}

	void Update () {

		if (player != null) {
			
			if (player.transform.position.y - transform.position.y > 200)
				DestroyObject (this.gameObject);
			else {

				transform.Translate( Vector2.up * speed * Time.deltaTime);
			}
		}
	}
}
