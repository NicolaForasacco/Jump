using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour {

	public GameObject[] tilePrefabs,
						collectablesPrefabs,
						animals;

	public Object flowerPrefab;
	public float tileWidth = 20, lastXPosition;

	public int maxPlatformLength = 1,
				minPlatformLength = 1,
				maxPlatformPerLevel = 2,
				randomRange = 4;



	private GameObject player;

	private int lastTileType,
				levels = 15,
				maxXRage = 4;

	private float zerop, ninep, onep, sixp, twofourp, sevenp, fivep, eightp;

	private float lastPlatformLength, 
					randomX, 
					lastPlatformPosition, 
					level = 40, 
					minLevelRange = 40f, 
					maxLevelRange = 40, 
					collectableP = 0.7f, 
					flowerP = 0.8f;

	private bool firstAnimalSpawn = false;


	private void changeParameters(){

		//maxPlatformLength = Random.Range (1,5);
		//minPlatformLength = Random.Range (1, maxPlatformLength);

		//maxPlatformPerLevel = Random.Range(1, 5 - maxPlatformLength);

		//randomRange = 
	}

	void Update () {
		
		if (player != null) {
			
			if (player.transform.position.y + 15 * tileWidth > level)
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

		/*if (level >= 20)
			changeParameters ();*/

		spawnPlatform (true);

		//se l'ultima tile non è una moving tile provo a creare un'altra tile in questo stesso livello
		if(lastTileType < 2 || lastTileType > 5)
			if(Random.value > 0.3)
				spawnPlatform (false);
		
		level += Random.Range(minLevelRange, maxLevelRange);
	}

	void setTilePercentage(){
		
		zerop = 0.45f;
		ninep = 0.05f;
		onep = 0.2f;
		sixp = 0.15f;
		twofourp = 0.08f;
		eightp = 0.02f;
		sevenp = 0.01f;
		fivep = 0.04f;
	}

	 
	void Start(){
		
		//maxXRage = randomRange;
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

			float currentRandomX = Random.Range (-randomRange + 1, randomRange);

			if (randomRange - Mathf.Abs (currentRandomX) < 2 && randomRange > 1)
				randomRange -= 1;
			else 
				randomRange = (randomRange < maxXRage) ? randomRange + 1 : maxXRage;

			randomX = lastPlatformPosition + currentRandomX * tileWidth;
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

			spawnAnimal (v2, tileType);
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

			Instantiate (flowerPrefab, v, Quaternion.identity);

			return true;
		}

		return false;
	}

	private bool spawnAnimal(Vector3 v, int tileType){

		if (tileType == 0 && Random.value <= 0.9f && !firstAnimalSpawn && v.y >= 100) {

			v.y = v.y + tileWidth;
			v.z = -1;
			int animalType = Random.Range (0, animals.Length);

			Instantiate (animals[animalType], v, Quaternion.identity);
			firstAnimalSpawn = true;

			return true;

		}else if (tileType == 0 && Random.value <= 0.05f && v.y >= 100) {

			v.y = v.y + tileWidth;
			v.z = -1;
			int animalType = Random.Range (0, animals.Length);

			Instantiate (animals[animalType], v, Quaternion.identity);

			if(Random.value <= 0.7f)
				Instantiate (animals[animalType], new Vector3(v.x + 4, v.y, v.z), Quaternion.identity);
			
			return true;
		}

		return false;
	}

	public float getAverageX(){
		
		return (player.transform.position.x + lastXPosition) / 2;
	}
}

