using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTileManager : MonoBehaviour {

	public GameObject[] animals;
	public GameObject pencilPrefab, shurikenPrefab, donutPrefab;
	public CameraController cameraController;
	public Tile[] tilePrefabs;
	public Object flowerPrefab;
	public float probabilityChangeEvery, donutP = 0.2f;
	public bool pencilSpawnThisLevel = true, shurikenSpawnThisLevel = true;


	private GameObject player;
	private Tile lastTileType;
	private float lastPlatformLength, lastPlatformPosition, currentLevelHeight;

	private int numberOfPlatformLevelSpawnedEachTime = 15, 
				maxXRage = 4,
				tileWidth = 20, 
				levelIndex = 0,
				randomXRange = 0,
				maxPlatformLength = 4,
				minPlatformLength = 1;

	private float flowerP = 0.6f,
					animalP = 0.3f,
					enemiesP = 0,
					lastXPosition = 1f,
					levelHeightRange = 40f,
					twoPlatformOnSameLevelP = 0.9f,
					enemiesPChangeParameter = 0.05f,
					minProbabilityChangeEvery = 160;

	void Start(){
		
		currentLevelHeight = levelHeightRange;
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

	private void changeProbabilityParameters(){
		
		levelIndex++;

		if (probabilityChangeEvery * 0.8f > minProbabilityChangeEvery)
			probabilityChangeEvery *= 0.8f;
		else
			probabilityChangeEvery = minProbabilityChangeEvery;

		cameraController.increaseSpeed ();

		foreach (Tile t in tilePrefabs)
			t.changeProbability ();

		twoPlatformOnSameLevelP -= 0.05f;

		enemiesP += enemiesPChangeParameter;
	}

	#region spawnPlatforms

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
		
		currentLevelHeight += levelHeightRange;
	}

	private void spawnPlatform(bool isFirstTileOfThisLevel){

		Tile tileType = null;
		float randomX;

		lastTileType = tileType = getNextTile(isFirstTileOfThisLevel);

		int platformLength = 1;

		if (!isFirstTileOfThisLevel) {

			platformLength = Random.Range (minPlatformLength, maxPlatformLength - 1);

			if (Random.value > 0.5f)
				randomX = lastPlatformPosition + lastPlatformLength * tileWidth + Random.Range (1, 3) * tileWidth;
			else
				randomX = lastPlatformPosition - platformLength * tileWidth - Random.Range (1, 3) * tileWidth;

		} else {

			platformLength = Random.Range (minPlatformLength, maxPlatformLength);

			float currentRandomX = Random.Range (-randomXRange + 1, randomXRange);

			if (randomXRange - Mathf.Abs (currentRandomX) < 2 && randomXRange > 1)
				randomXRange -= 1;
			else 
				randomXRange = (randomXRange < maxXRage) ? randomXRange + 1 : maxXRage;

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

			if(!spawnDonut (tmpPosition, tileType.type))
			if(!spawnFlowers (tmpPosition, tileType.type))
				spawnAnimal (tmpPosition, tileType.type);
		}

		lastXPosition = tmpPosition.x;
	}

	private Tile getNextTile(bool isFirstTileOfThisLevel){

		float randomFloat = Random.Range (0, getCurrentTotalTilesP()), currentP = 0;
		Tile tileType = null;

		for(int i = 0; i < tilePrefabs.Length && tileType == null; i++){

			currentP += tilePrefabs[i].currentProbability;

			if (randomFloat <= currentP) {

				switch(tilePrefabs[i].type){

				case Tile.TileType.MovingTile1:
				case Tile.TileType.MovingTile2:
				case Tile.TileType.MovingTile3:

					if (isFirstTileOfThisLevel)
						tileType = tilePrefabs[i];
					else
						i = tilePrefabs.Length;    // se non va bene la moving tile perchè non è la prima del livello esco dal for

					break;
				case Tile.TileType.SpineTile:

					if (!isFirstTileOfThisLevel)
						tileType = tilePrefabs[i];
					else
						i = tilePrefabs.Length;    // se non va bene la spine tile perchè è la prima del livello esco dal for

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
		
	private float getCurrentTotalTilesP(){
		
		float totalP = 0;

		for (int i = 0; i < tilePrefabs.Length; i++)
			totalP += tilePrefabs [i].currentProbability;
		
		return totalP;
	}

	#endregion

	#region spawnObjects

	private bool spawnDonut(Vector3 position, Tile.TileType tileType){
		
		if (tileType != Tile.TileType.SpineTile &&
			tileType != Tile.TileType.MovingTile1 && 
			tileType != Tile.TileType.MovingTile2 &&
			tileType != Tile.TileType.MovingTile3 &&
			tileType != Tile.TileType.ColorizeTile &&
			Random.value <= donutP) {

			position.y = position.y + tileWidth;
			position.z = -1;

			Instantiate (donutPrefab, position, Quaternion.identity);

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

		if (tileType == Tile.TileType.NormalTile && Random.value <= animalP && position.y >= 100) {

			position.y = position.y + tileWidth;
			position.z = -1;

			int animalType = Random.Range (0, animals.Length);
			int animalNumber = Random.Range (1, 4);

			for (int i = 0; i < animalNumber; i++)
				Instantiate (animals[animalType], new Vector3(position.x + 2 * i, position.y, position.z), Quaternion.identity);
			
			return true;
		}else
			return false;
	}

	#endregion

	#region spawnEnemies

	public void spawnEnemies(){

		if (Random.value <= enemiesP) {
			float r = Random.value;

			if (Random.value <= 0.5 && !shurikenSpawnThisLevel)
				spawnRandomShuriken ();
			else if (!pencilSpawnThisLevel)
				spawnRandomHorizontalPencil ();
		}
	}

	private void spawnRandomHorizontalPencil(){

		pencilSpawnThisLevel = true;
		Vector2 v = new Vector2 (cameraController.getXBounds(), ((int)(player.transform.position.y + Random.Range(7, 14) * tileWidth) / 20) * 20 );
		Instantiate (pencilPrefab, v, Quaternion.identity);
	}

	private void spawnRandomShuriken(){

		shurikenSpawnThisLevel = true;
		Vector2 v = new Vector2 (cameraController.getXStart(), player.transform.position.y + Random.Range(7, 14) * tileWidth);
		Instantiate (shurikenPrefab, v, Quaternion.identity);
	}

	#endregion

	public float getAverageX(){
		
		return (player.transform.position.x + lastXPosition) / 2;
	}
}