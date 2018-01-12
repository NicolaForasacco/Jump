using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudDestroyer : MonoBehaviour {
	
	GameObject player;
	Vector3 v;

	float speedY;

	void Start () {
		
		transform.SetParent (GameObject.FindGameObjectWithTag ("background").transform);
		player = GameObject.FindGameObjectWithTag ("player");

		float randomScale = Random.Range (1,3);

		transform.localScale = (Random.value > 0.5) ? new Vector2 (randomScale, randomScale): new Vector2 (-randomScale, randomScale);

		(GetComponent<SpriteRenderer> ()).color = new Color (1f, 1f, 1f, Random.Range(0.5f, 0.9f));

		speedX = Random.Range (- 13, 15);
		speedX *= 4;

		v = transform.position;
	}

	float speedX;

	Vector3 velocity;

	void Update () {

		if (player != null) {
			
			if (player.transform.position.y - transform.position.y > 200 || Mathf.Abs (player.transform.position.x - transform.position.x) > 300)
				DestroyObject (this.gameObject);
			else {
				
				v.x = v.x + speedX * Time.deltaTime;

				v = Vector3.SmoothDamp (transform.position, v, ref velocity, 0.1f);

				transform.position = v;
			}
		}
	}
}
