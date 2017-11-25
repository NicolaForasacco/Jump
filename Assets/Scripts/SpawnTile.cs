using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour {

	public GameObject player;
	public GameObject[] tilePrefabs;
	public GameObject[] collectablesPrefabs;

	public int maxPlatformLength = 1;
	public int minPlatformLength = 1;
	public int maxPlatformPerLevel = 2;
	public int randomRange = 7;

	private void changeParameters(){

		//maxPlatformLength = Random.Range (1,5);
		//minPlatformLength = Random.Range (1, maxPlatformLength);

		//maxPlatformPerLevel = Random.Range(1, 5 - maxPlatformLength);

		//randomRange = 
	}

	public int maxCollectablesPerLevel = 2;

	float lastPlatformPosition;

	private float level = 40;

	void Start () {
		
		lastPlatformPosition = 0;
		lastPlatformPosition = player.transform.position.x;
	}

	public void spawnMultiplePlatform(){
		
		for (int i = 0; i < 50; i++)
			spawnALevelOfPlatforms ();
	}

	public void spawnALevelOfPlatforms(){

		if (level >= 20)
			changeParameters ();
		
		for (int i = 0; i < maxPlatformPerLevel; i++) 
			spawnPlatform ();
		
		level += Random.Range(35f, 40f);
	}

	void spawnPlatform(){

		int platformLength = Random.Range (minPlatformLength, maxPlatformLength);

		float randomX = lastPlatformPosition + Random.Range (- randomRange + 1, randomRange)*10;

		lastPlatformPosition = randomX;


		int tileType = 0;
		float randomFloat = Random.value;

		if (randomFloat <= 0.4f)
			tileType = 0;
		else if (randomFloat <= 0.6f)
			tileType = 1;
		else if (randomFloat <= 0.8f)
			tileType = 6;
		else if (randomFloat <= 0.9f)
			tileType = Random.Range (2, 5);
		else if (randomFloat <= 1)
			tileType = 5;

		Vector2 v2;

		if (tileType < 2 || tileType > 5)
			platformLength = Random.Range (1, 4);
		
		for (int i = 0; i < platformLength; i++) {
			
			v2 = new Vector2 (randomX + i*20, level);
			Instantiate (tilePrefabs [tileType], v2, Quaternion.identity);

			if (tileType < 2 || tileType > 5)
				spawnCollectable (v2);
		}
	}

	private void spawnCollectable(Vector2 v){

		if (Random.value > 0.8) {
			v.y = v.y + 20;
			Instantiate (collectablesPrefabs[Random.Range(0,collectablesPrefabs.Length)], v, Quaternion.identity);
		}
	}
}

