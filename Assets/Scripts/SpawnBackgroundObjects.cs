using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBackgroundObjects : MonoBehaviour {

	public GameObject cloudPrefab;
	[UnityEngine.Serialization.FormerlySerializedAs("baloonPrefab")]
	public GameObject balloonPrefab;

	public float cloudP = 0.9f;
	[UnityEngine.Serialization.FormerlySerializedAs("ballonP")]
	public float balloonP = 0.5f;

	private GameObject player;
	private float lastSpawnPositionY = 0;

	private int tileWidth = 20, minXRange = -6, maxXRange = 6, minYRange = 5, maxYRange = 8;

	void Start () {
		
		player = GameObject.FindGameObjectWithTag ("player");
	}

	void Update () {
		
		if (player != null) {
			if (player.transform.position.y + 300 > lastSpawnPositionY)
				spawnBackgroundObject ();
		}else
			player = GameObject.FindGameObjectWithTag ("player");
	}

	float getRandomX(){
		return (GameObject.FindGameObjectWithTag ("GameController").GetComponent<SpawnTileManager> ().getAverageX()) + Random.Range (minXRange, maxXRange) * tileWidth;
	}

	float getRandomY(){
		return lastSpawnPositionY + Random.Range (minYRange, maxYRange) * tileWidth;
	}
		
	void spawnBackgroundObject(){
		
		Vector3 randomPosition = new Vector3 (getRandomX(), getRandomY(), 1);

		if (Random.value <= cloudP)
			Instantiate (cloudPrefab, randomPosition, Quaternion.identity);
		else if (Random.value <= balloonP)
				Instantiate (balloonPrefab, randomPosition, Quaternion.identity);

		lastSpawnPositionY = randomPosition.y;
	}
}
