using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

	int side = 1;
	float speedX = 100;
	float speedY = 200;

	bool isMoving = false;

	GameObject player;

	Vector3 initialPosition;

	void Start () {

		player = GameObject.FindGameObjectWithTag ("player");
		transform.SetParent (GameObject.FindGameObjectWithTag ("foreground").transform);

		side = (GameObject.FindGameObjectWithTag ("GameController").GetComponent<SpawnTileManager> ().lastXPosition > 0) ? 1 : -1;

		float randomScale = Random.Range (0.3f, 0.8f);

		transform.localScale = new Vector2 (side * randomScale, 1 * randomScale);
		initialPosition = transform.position;
	}

	void Update () {
		
		if (isMoving) {
			
			transform.position += Vector3.right * side * Time.deltaTime * speedX;
			float r = Random.value;

			if (r > 0.2f)
				transform.position += Vector3.up * Time.deltaTime * speedY;
			else if (r > 0f)
				transform.position += Vector3.down * Time.deltaTime * speedY * 0.8f;

			if ((transform.position.x >= initialPosition.x + 400 || transform.position.x <= initialPosition.x - 400) && transform.position.y >= initialPosition.y + 250)
				DestroyObject (this.gameObject);
		} else {
			
			if (player != null) {
				if (transform.position.y - player.transform.position.y < 100 && transform.position.x - player.transform.position.x < 100)
					isMoving = true;
			}
		}
	}
}
