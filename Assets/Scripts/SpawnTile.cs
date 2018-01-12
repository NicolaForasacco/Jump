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
	public float lastXPosition;

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
		else if (randomFloat <= 0.45f && n == 0)
			tileType = 10;
        else if (randomFloat <= 0.5)
            tileType = 9;
        else if (randomFloat <= 0.6f)
            tileType = 1;
        else if (randomFloat <= 0.75f)
            tileType = 6;
        else if (randomFloat <= 0.85f && n == 0)
            tileType = Random.Range(2, 5);
        else if (randomFloat <= 0.9f)
            tileType = 1;
        else if (randomFloat <= 0.93f)
            tileType = 7;
        else if (randomFloat <= 0.96f)
            tileType = 0;
        else if (randomFloat <= 1)
            tileType = 5;

		lastTileType = tileType;

		int platformLength = 1;

		if (n != 0) {
		
			platformLength = Random.Range (1, 3);

			if (Random.value > 0.5f)
				randomX = lastPlatformPosition + lastPlatformLength * tileWidth + Random.Range (1, 3) * tileWidth;
			else
				randomX = lastPlatformPosition - platformLength * tileWidth - Random.Range (1, 3) * tileWidth;
		} else {
			
			if (tileType < 2 || tileType > 5 && tileType != 10)
				platformLength = Random.Range (1, 4);
			
			randomX = lastPlatformPosition + Random.Range (- randomRange + 1, randomRange) * tileWidth;
			lastPlatformPosition = randomX;
		}

		if (tileType == 7)
			platformLength = 1;
		
		lastPlatformLength = platformLength;


		Vector2 v2 = new Vector2 (randomX, level);

		for (int i = 0; i < platformLength; i++) {
			
			v2 = new Vector3 (randomX + i * tileWidth, level, 0);
			Instantiate (tilePrefabs [tileType], v2, Quaternion.identity);

			if(!spawnFlowers (v2, tileType))
				spawnCollectable (v2, tileType);
		}

		lastXPosition = v2.x;
	}
	public float collectableN = 0.7f;

	private void spawnCollectable(Vector3 v, int tileType){
		
		float currentN = collectableN;

		if (tileType == 5)
			currentN -= 0.4f;

		if ((tileType < 2 || tileType > 5) && tileType != 7) {

			if (Random.value > 1 - collectableN) {
			
				v.y = v.y + tileWidth - 2;
				v.z = -1;
				int foodType = Random.Range (0, collectablesPrefabs.Length);

				Instantiate (collectablesPrefabs [foodType], v, Quaternion.identity);
			}
		}
	}

	public GameObject[] flowersPrefab;

	private bool spawnFlowers(Vector3 v, int tileType){


		if (tileType == 0 && Random.value > 0.1f) {
			
			v.y = v.y + tileWidth / 2;
			v.z = -1;
			
			int flowerType = Random.Range (0, flowersPrefab.Length);
			Instantiate (flowersPrefab [flowerType], v, Quaternion.identity);
			return true;
		}

		return false;
	}
}

