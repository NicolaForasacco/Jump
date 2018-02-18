using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundWorldDestroyer : MonoBehaviour {

	public Sprite[] backgroundWorlds;
	GameObject player;

	float 	minScale = 2f,
			maxScale = 2f,
			minAlpha = 0.15f,
			maxAlpha = 0.3f;

	int maxDistanceFromPlayer = 300;

	void Start () {

		transform.SetParent (GameObject.FindGameObjectWithTag ("background").transform);
		player = GameObject.FindGameObjectWithTag ("player");

		GetComponent<SpriteRenderer> ().sprite = backgroundWorlds [Random.Range (0, backgroundWorlds.Length - 1)];

		float randomScale = Random.Range (minScale, maxScale);
		int side =  (Random.value > 0.5f) ? 1: -1;
		transform.localScale = new Vector2 (randomScale * side, randomScale);

		(GetComponent<SpriteRenderer> ()).color = new Color (1f, 1f, 1f, Random.Range(minAlpha, maxAlpha));
	}



	void Update () {

		if (player != null)
		if (player.transform.position.y - transform.position.y > maxDistanceFromPlayer ||
			Mathf.Abs (player.transform.position.x - transform.position.x) > maxDistanceFromPlayer)
			DestroyObject (this.gameObject);
	
	}
}
