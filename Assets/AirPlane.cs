using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlane : MonoBehaviour {



	public Sprite[] planeSprites;

	GameObject player;
	Vector3 lastUpdatedPosition, velocity;

	float speedY, speedX, minAlpha = 0.5f, maxAlpha = 0.9f, minXSpeed = 80, maxXSpeed = 220;

	float minScale = 0.5f, maxScale = 0.9f, maxDistanceFromPlayer = 300;


	void Start () {

		transform.SetParent (GameObject.FindGameObjectWithTag ("background").transform);
		player = GameObject.FindGameObjectWithTag ("player");


		GetComponent<SpriteRenderer> ().sprite = planeSprites [Random.Range (0, planeSprites.Length - 1)];

		float lastXPosition = (GameObject.FindGameObjectWithTag ("GameController").GetComponent<SpawnTile> ().getAverageX());
		float randomScale = Random.Range (minScale, maxScale);

		int side = (lastXPosition > transform.position.x) ? 1 : -1;
		transform.localScale = new Vector2 (randomScale * side, randomScale);


		(GetComponent<SpriteRenderer> ()).color = new Color (1f, 1f, 1f, Random.Range(minAlpha, maxAlpha));

    	speedX = Random.Range (minXSpeed, maxXSpeed) * 4 * side;

		lastUpdatedPosition = transform.position;
	}


	void Update () {

		if (player != null) {

			if (player.transform.position.y - transform.position.y > maxDistanceFromPlayer || Mathf.Abs (player.transform.position.x - transform.position.x) > maxDistanceFromPlayer) {

				DestroyObject (this.gameObject);

			}else if (transform.position.y - player.transform.position.y < 160) {
					lastUpdatedPosition.x = lastUpdatedPosition.x + speedX * Time.deltaTime;

					lastUpdatedPosition = Vector3.SmoothDamp (transform.position, lastUpdatedPosition, ref velocity, 0.1f);

					transform.position = lastUpdatedPosition;
			}
		}
	}
}