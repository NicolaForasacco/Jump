using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudDestroyer : MonoBehaviour {
	

	public Sprite[] cloudSprites;

	GameObject player;
	Vector3 lastUpdatedPosition, velocity;

	float speedY, speedX, minAlpha = 0.5f, maxAlpha = 0.9f, minXSpeed = 0, maxXSpeed = 15;

	int minScale = 1, maxScale = 3, maxDistanceFromPlayer = 300;

	void Start () {
		
		transform.SetParent (GameObject.FindGameObjectWithTag ("background").transform);
		player = GameObject.FindGameObjectWithTag ("player");


		GetComponent<SpriteRenderer> ().sprite = cloudSprites [Random.Range (0, cloudSprites.Length - 1)];


		float randomScale = Random.Range (minScale, maxScale);
		int side = (Random.value > 0.5) ? 1 : -1;
		transform.localScale = new Vector2 (randomScale * side, randomScale);


		(GetComponent<SpriteRenderer> ()).color = new Color (1f, 1f, 1f, Random.Range(minAlpha, maxAlpha));


		float lastXPosition = (GameObject.FindGameObjectWithTag ("GameController").GetComponent<SpawnTileManager> ().getAverageX());
		speedX = ((lastXPosition > transform.position.x) ? Random.Range (-maxXSpeed, minXSpeed) : Random.Range (minXSpeed, maxXSpeed)) * 4;

		lastUpdatedPosition = transform.position;
	}

		
	void Update () {

		if (player != null) {
			
			if (player.transform.position.y - transform.position.y > maxDistanceFromPlayer || Mathf.Abs (player.transform.position.x - transform.position.x) > maxDistanceFromPlayer) {

				DestroyObject (this.gameObject);

			}else {
				
				lastUpdatedPosition.x = lastUpdatedPosition.x + speedX * Time.deltaTime;

				transform.position = lastUpdatedPosition = Vector3.SmoothDamp (transform.position, lastUpdatedPosition, ref velocity, 0.1f);
			}
		}
	}
}
