using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour {

	private GameObject player;
	public GameObject bird;

	public GameObject[] tilePrefabs, collectablesPrefabs, flowersPrefab;

	public float tileWidth = 20;

	public int maxPlatformLength = 1, minPlatformLength = 1, maxPlatformPerLevel = 2, randomRange = 5;

	public float lastXPosition;

	float lastPlatformLength, randomX, lastPlatformPosition;

	int lastTileType;
	int levels = 15;

	private float level = 40;
	private float zerop, ninep, onep, sixp, twofourp, sevenp, fivep, eightp, collectableP = 0.7f, flowerP = 0.8f;

	bool birdSpawn = false;


	private void changeParameters(){

		//maxPlatformLength = Random.Range (1,5);
		//minPlatformLength = Random.Range (1, maxPlatformLength);

		//maxPlatformPerLevel = Random.Range(1, 5 - maxPlatformLength);

		//randomRange = 
	}

	void Update () {
		
		if (player != null) {
			if (player.transform.position.y + 300 > level)
				spawnMultiplePlatform ();
		} else {
			
			player = GameObject.FindGameObjectWithTag ("player");
			lastPlatformPosition = 0;
			lastPlatformPosition = player.transform.position.x;
		}
	}

	public void spawnMultiplePlatform(){

		for (int i = 0; i < levels; i++)
			spawnALevelOfPlatforms ();

		levels = 2;
	}

	public void spawnALevelOfPlatforms(){

		if (level >= 20)
			changeParameters ();

		spawnPlatform (true);

		//se l'ultima tile non è una moving tile provo a creare un'altra tile in questo stesso livello
		if(lastTileType < 2 || lastTileType > 5)
			if(Random.value > 0.3)
				spawnPlatform (false);
		
		level += Random.Range(40f, 40f);
	}

	void setTilePercentage(){
		
		zerop = 0.4f;
		ninep = 0.13f;
		onep = 0.15f;
		sixp = 0.15f;
		twofourp = 0.08f;
		eightp = 0.02f;
		sevenp = 0.03f;
		fivep = 0.04f;
	}
	 
	void spawnPlatform(bool isFirstTileOfThisLevel){

		int tileType = 0;
		float randomFloat = Random.value;

		setTilePercentage ();

		if (randomFloat <= zerop)
			tileType = 0;
		else if (randomFloat <= zerop + ninep)
			tileType = 9;
		else if (randomFloat <= zerop + ninep + onep)
			tileType = 1;
		else if (randomFloat <= zerop + ninep + onep + sixp)
			tileType = 6;
		else if (randomFloat <= zerop + ninep + onep + sixp + twofourp && isFirstTileOfThisLevel)
			tileType = Random.Range (2, 5);
		else if (randomFloat <= zerop + ninep + onep + sixp + twofourp + sevenp)
			tileType = 7;
		else if (randomFloat <= zerop + ninep + onep + sixp + twofourp + sevenp + fivep)
			tileType = 5;
		else if (randomFloat <= zerop + ninep + onep + sixp + twofourp + sevenp + fivep + eightp)
			tileType = 8;

		lastTileType = tileType;

		int platformLength = 1;

		if (!isFirstTileOfThisLevel) {
		
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


		Vector2 v2 = new Vector2 (randomX, level);

		for (int i = 0; i < platformLength; i++) {
			
			v2 = new Vector3 (randomX + i * tileWidth, level, 0);
			Instantiate (tilePrefabs [tileType], v2, Quaternion.identity);

			if(!spawnFlowers (v2, tileType))
				spawnCollectable (v2, tileType);

			spawnBird (v2, tileType);
		}

		lastXPosition = v2.x;
	}

	private void spawnCollectable(Vector3 v, int tileType){
		
		if ((tileType < 2 || tileType > 5) && tileType != 7 && tileType != 5) {

			if (Random.value <= collectableP) {
			
				v.y = v.y + tileWidth - 2;
				v.z = -1;
				int foodType = Random.Range (0, collectablesPrefabs.Length);

				Instantiate (collectablesPrefabs [foodType], v, Quaternion.identity);
			}
		}
	}

	private bool spawnFlowers(Vector3 v, int tileType){


		if (tileType == 0 && Random.value <= flowerP) {
			
			v.y = v.y + tileWidth / 2;
			v.z = -1;
			
			int flowerType = Random.Range (0, flowersPrefab.Length);
			Instantiate (flowersPrefab [flowerType], v, Quaternion.identity);

			return true;
		}

		return false;
	}

	private bool spawnBird(Vector3 v, int tileType){

		if (tileType == 0 && Random.value > 0.1f && !birdSpawn && v.y >= 100) {

			v.y = v.y + tileWidth;
			v.z = -1;

			Instantiate (bird, v, Quaternion.identity);
			birdSpawn = true;
			return true;
		}

		return false;
	}
}

