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
	public int randomRange = 5;

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

	void Update () {

		if (player.transform.position.y + 300 > level)
			spawnMultiplePlatform ();
	}


	int levels = 15;

	public void spawnMultiplePlatform(){

		for (int i = 0; i < levels; i++)
			spawnALevelOfPlatforms ();

		levels = 2;
	}

	public void spawnALevelOfPlatforms(){

		if (level >= 20)
			changeParameters ();

		spawnPlatform (0);

		if(lastTileType < 2 || lastTileType > 5)
			if(Random.value > 0.3)
				spawnPlatform (1);
		
		level += Random.Range(40f, 40f);
	}

	float lastPlatformLength;

	float randomX;

	int lastTileType;

	public float tileWidth = 20;

	void spawnPlatform(int n){

		int tileType = 0;
		float randomFloat = Random.value;

		if (randomFloat <= 0.4f)
			tileType = 0;
		else if (randomFloat <= 0.6f)
			tileType = 1;
		else if (randomFloat <= 0.75f)
			tileType = 6;
		else if (randomFloat <= 0.85f && n == 0)
			tileType = Random.Range (2, 5);
		else if (randomFloat <= 0.9f)
			tileType = 1;
		else if (randomFloat <= 0.93f)
			tileType = 7;
		else if (randomFloat <= 0.96f)
			tileType = 8;

		lastTileType = tileType;

		int platformLength = 1;

		if (n != 0) {
		
			platformLength = Random.Range (1, 3);

			if (Random.value > 0.5f)
				randomX = lastPlatformPosition + lastPlatformLength * tileWidth + Random.Range (1, 3) * tileWidth;
			else
				randomX = lastPlatformPosition - platformLength * tileWidth - Random.Range (1, 3) * tileWidth;
		} else {
			
			if (tileType < 2 || tileType > 5)
				platformLength = Random.Range (1, 4);
			
			randomX = lastPlatformPosition + Random.Range (- randomRange + 1, randomRange) * tileWidth;
			lastPlatformPosition = randomX;
		}

		if (tileType == 7)
			platformLength = 1;
		
		lastPlatformLength = platformLength;


		Vector2 v2;
		for (int i = 0; i < platformLength; i++) {
			
			v2 = new Vector2 (randomX + i * tileWidth, level);
			Instantiate (tilePrefabs [tileType], v2, Quaternion.identity);

			if ((tileType < 2 || tileType > 5) && tileType != 7 && tileType != 8)
				spawnCollectable (v2);
		}
	}

	private void spawnCollectable(Vector2 v){

		if (Random.value > 0.3) {
			v.y = v.y + tileWidth - 2;
			int foodType = Random.Range (0, collectablesPrefabs.Length);

			Instantiate (collectablesPrefabs[foodType], v, Quaternion.identity);
		}
	}
}

