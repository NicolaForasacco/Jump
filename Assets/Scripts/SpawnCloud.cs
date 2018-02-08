using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCloud : MonoBehaviour {

	private GameObject player;

	public GameObject cloudPrefab, backgroundPrefab, baloonPrefab;

	float lastSpawnPositionY = 0;

	int tileWidth = 20, minXRange = -6, maxXRange = 6, minYRange = 5, maxYRange = 8;

	void Start () {
		
		player = GameObject.FindGameObjectWithTag ("player");
	}

	void Update () {
		
		if (player != null) {
			
			if (player.transform.position.y + 300 > lastSpawnPositionY)
				spawnClouds ();
		}else
			player = GameObject.FindGameObjectWithTag ("player");
	}

	float getRandomX(){
		
		return (GameObject.FindGameObjectWithTag ("GameController").GetComponent<SpawnTile> ().getAverageX()) + Random.Range (minXRange, maxXRange) * tileWidth;
	}

	float getRandomSideX(){
		
		if(Random.value <= 0.5f)
			return (GameObject.FindGameObjectWithTag ("GameController").GetComponent<SpawnTile> ().getAverageX()) - 1 * maxXRange * tileWidth;
		else
			return (GameObject.FindGameObjectWithTag ("GameController").GetComponent<SpawnTile> ().getAverageX()) + 1 * maxXRange * tileWidth;
	}

	float getRandomY(){
		return lastSpawnPositionY + Random.Range (minYRange, maxYRange) * tileWidth;
	}

	void spawnClouds(){
		
		Vector3 randomPosition = new Vector3 (getRandomX(), getRandomY(), 1);

		if (Random.value <= 0.7f)
			Instantiate (cloudPrefab, randomPosition, Quaternion.identity);
		else {

			randomPosition = new Vector3 (getRandomSideX(), getRandomY(), 1);

			if (Random.value <= 0.8f)
				Instantiate (backgroundPrefab, randomPosition, Quaternion.identity);
		}

		lastSpawnPositionY = randomPosition.y;
	}
}
