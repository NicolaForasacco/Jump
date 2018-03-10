using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour {

	public GameObject[] collectablesPrefabs,
						animals;

	public Tile[] tilePrefabs;

	public Object flowerPrefab;
	public float tileWidth = 20, 
				lastXPosition;

	public int	maxPlatformLength = 4,
				minPlatformLength = 1,
				randomRange	= 3;


	private GameObject player;

	private int numberOfPlatformLevelSpawnedEachTime	= 15,
				maxXRage	= 4;

	private Tile lastTileType;

	private float 	lastPlatformLength, 
					randomX, 
					lastPlatformPosition,
					minLevelHeightRange =	40f, 
					maxLevelHeightRange =	60f, 
					collectableP = 0.1f, 
					flowerP = 0.6f,
					animalP = 0.3f,
					currentLevelHeight = 40,
					twoPlatformOnSameLevelP = 0.8f;

	int levelIndex = 0;

	float probabilityChangeEvery = 800;

	void changeProbabilityParameters(){

		Debug.Log ("Probabilities changed at " + currentLevelHeight);

		levelIndex++;
		probabilityChangeEvery *= 1.5f;

		for (int i = 0; i < tilePrefabs.Length; i++)
			tilePrefabs [i].changeProbability ();

		if (minLevelHeightRange * 1.05f <= maxLevelHeightRange) {
			minLevelHeightRange *= 1.05f;
		}
	}

	void Update () {
		
		if (player != null) {

			if (currentLevelHeight >= (levelIndex + 1) * probabilityChangeEvery)
				changeProbabilityParameters ();

			if (player.transform.position.y + 15 * tileWidth > currentLevelHeight)
				spawnMultiplePlatform ();
		} else {
			
			player = GameObject.FindGameObjectWithTag ("player");
			lastPlatformPosition = 0;
			lastPlatformPosition = player.transform.position.x;
		}
	}

	public void spawnMultiplePlatform(){

		for (int i = 0; i < numberOfPlatformLevelSpawnedEachTime; i++)
			spawnALevelOfPlatforms ();

		numberOfPlatformLevelSpawnedEachTime = 2;
	}

	public void spawnALevelOfPlatforms(){
		
		spawnPlatform (true);

		//se l'ultima tile non è una moving tile provo a creare un'altra tile in questo stesso livello
		if(lastTileType.type != Tile.TileType.MovingTile1 && 
			lastTileType.type != Tile.TileType.MovingTile2 &&
			lastTileType.type != Tile.TileType.MovingTile3)
		if(Random.value < twoPlatformOnSameLevelP)
			spawnPlatform (false);
		
		currentLevelHeight += minLevelHeightRange;//Random.Range(minLevelHeightRange, maxLevelHeightRange);
	}

	float getCurrentTotalTilesP(){
		
		float totalP = 0;

		for (int i = 0; i < tilePrefabs.Length; i++)
			totalP += tilePrefabs [i].currentProbability;
		
		return totalP;
	}

	Tile getNextTile(bool isFirstTileOfThisLevel){

		float randomFloat = Random.Range (0, getCurrentTotalTilesP());
		Tile tileType = null;
		float currentP = 0;

		for(int i = 0; i < tilePrefabs.Length && tileType == null; i++){

			currentP += tilePrefabs[i].currentProbability;

			if (randomFloat <= currentP) {

				switch(tilePrefabs[i].type){

				case Tile.TileType.MovingTile1:
				case Tile.TileType.MovingTile2:
				case Tile.TileType.MovingTile3:

					if (isFirstTileOfThisLevel)
						tileType = tilePrefabs [i];
					break;

				default:
					tileType = tilePrefabs[i];
					break;
				}
			}
		}

		if (tileType== null)
			tileType = tilePrefabs[0];

		return tileType;
	}

	void spawnPlatform(bool isFirstTileOfThisLevel){

		Tile tileType = null;

		tileType = getNextTile(isFirstTileOfThisLevel);
		lastTileType = tileType;

		int platformLength = 1;

		if (!isFirstTileOfThisLevel) {
		
			platformLength = Random.Range (minPlatformLength, maxPlatformLength - 1);

			if (Random.value > 0.5f)
				randomX = lastPlatformPosition + lastPlatformLength * tileWidth + Random.Range (1, 3) * tileWidth;
			else
				randomX = lastPlatformPosition - platformLength * tileWidth - Random.Range (1, 3) * tileWidth;
			
		} else {

			platformLength = Random.Range (minPlatformLength, maxPlatformLength);

			float currentRandomX = Random.Range (-randomRange + 1, randomRange);

			if (randomRange - Mathf.Abs (currentRandomX) < 2 && randomRange > 1)
				randomRange -= 1;
			else 
				randomRange = (randomRange < maxXRage) ? randomRange + 1 : maxXRage;

			randomX = lastPlatformPosition + currentRandomX * tileWidth;
			lastPlatformPosition = randomX;
		}
			
		if (tileType.type == Tile.TileType.ColorizeTile || 
			tileType.type == Tile.TileType.BounceTile ||
			tileType.type == Tile.TileType.MovingTile1 ||
			tileType.type == Tile.TileType.MovingTile2 ||
			tileType.type == Tile.TileType.MovingTile3)
			platformLength = 1;
		
		lastPlatformLength = platformLength;


		Vector2 tmpPosition = new Vector2 (randomX, currentLevelHeight);

		for (int i = 0; i < platformLength; i++) {
			
			tmpPosition = new Vector3 (randomX + i * tileWidth, currentLevelHeight, 0);

			Instantiate (tileType.tile, tmpPosition, Quaternion.identity);

			if(!spawnCollectable (tmpPosition, tileType.type))
			if(!spawnFlowers (tmpPosition, tileType.type))
				spawnAnimal (tmpPosition, tileType.type);
		}

		lastXPosition = tmpPosition.x;
	}



	float donutP = 0.9f, pizzaP = 0.05f, hamburgerP = 0.05f;

	private bool spawnCollectable(Vector3 position, Tile.TileType tileType){
		
		if (tileType != Tile.TileType.LiftTile && 
			tileType != Tile.TileType.SpineTile &&
			tileType != Tile.TileType.MovingTile1 && 
			tileType != Tile.TileType.MovingTile2 &&
			tileType != Tile.TileType.MovingTile3 && 
			tileType != Tile.TileType.ColorizeTile && 
			Random.value <= collectableP) {
			
				position.y = position.y + tileWidth;
				position.z = -1;
				float randomP = Random.value;

				if(randomP <= donutP)
					Instantiate (collectablesPrefabs [0], position, Quaternion.identity);
			
				return true;
		}else
			return false;
	}

	private bool spawnFlowers(Vector3 position, Tile.TileType tileType){


		if (tileType == Tile.TileType.NormalTile && Random.value <= flowerP) {
			
			position.y = position.y + tileWidth / 2;
			position.z = -1;

			Instantiate (flowerPrefab, position, Quaternion.identity);

			return true;
		}else
			return false;
	}

	private bool spawnAnimal(Vector3 position,  Tile.TileType tileType){

		if (tileType == Tile.TileType.NormalTile && Random.value <= animalP && position.y >= 100 && !player.GetComponent<PlayerMovement>().isOnLift) {

			position.y = position.y + tileWidth;
			position.z = -1;

			int animalType = Random.Range (0, animals.Length);
			int animalNumber = Random.Range (1, 4);

			for (int i = 0; i < animalNumber; i++)
				Instantiate (animals[animalType], new Vector3(position.x + 2*i, position.y, position.z), Quaternion.identity);
			

			return true;
		}else
			return false;
	}


	public float getAverageX(){
		
		return (player.transform.position.x + lastXPosition) / 2;
	}
}

