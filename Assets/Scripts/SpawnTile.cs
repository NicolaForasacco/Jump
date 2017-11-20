using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour {

	public GameObject player;
	public GameObject[] tilePrefabs;
	public GameObject[] collectablesPrefabs;

	public int maxPlatformLength = 1;
	public int minPlatformLength = 1;
	public int maxPlatformPerLevel = 1;
	public int randomRange = 7;

	private void changeParameters(){

		//maxPlatformLength = Random.Range (1,5);
		//minPlatformLength = Random.Range (1, maxPlatformLength);

		//maxPlatformPerLevel = Random.Range(1, 5 - maxPlatformLength);

		//randomRange = 
	}

	public int maxCollectablesPerLevel = 2;

	float lastPlatformPosition;

	private int level = 2;

	void Start () {
		
		lastPlatformPosition = 0;
		lastPlatformPosition = player.transform.position.x;
	}

	public void spawnMultiplePlatform(){
		
		for (int i = 0; i < 100; i++)
			spawnALevelOfPlatforms ();
	}

	public void spawnALevelOfPlatforms(){

		if (level >= 20)
			changeParameters ();
		
		for (int i = 0; i < maxPlatformPerLevel; i++) 
			spawnPlatform ();
		
		level += 2;
	}

	void spawnPlatform(){

		int platformLength = Random.Range (minPlatformLength, maxPlatformLength);


		int randomX = Mathf.RoundToInt (lastPlatformPosition + Random.Range (- randomRange + 1, randomRange));
		int randomY = Mathf.RoundToInt (level);

		lastPlatformPosition = randomX;


		int tileType = Random.Range (0, tilePrefabs.Length);

		Vector2 v2;
		for (int i = 0; i <platformLength; i++) {
			
			v2 = new Vector2 (randomX + i, randomY);
			Instantiate (tilePrefabs [tileType], v2, Quaternion.identity);

			spawnCollectable (v2);
		}
	}

	private void spawnCollectable(Vector2 v){

		if (Random.value > 0.8) {
			v.y = v.y + 1;
			Instantiate (collectablesPrefabs[Random.Range(0,collectablesPrefabs.Length)], v, Quaternion.identity);
		}
	}
}

