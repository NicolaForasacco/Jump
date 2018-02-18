using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour {

	public GameObject[] collectablesPrefabs,
						animals;

	public Tile[] tilePrefabs;

	public Object flowerPrefab;
	public float tileWidth = 20, lastXPosition;

	public int	maxPlatformLength	= 1,
				minPlatformLength	= 1,
				maxPlatformPerLevel	= 2,
				randomRange	= 4;



	private GameObject player;

	private int levels	= 15,
				maxXRage	= 4;

	private Tile lastTileType;

	private float zerop, ninep, onep, sixp, twofourp, sevenp, fivep, eightp;

	private float 	lastPlatformLength, 
					randomX, 
					lastPlatformPosition,
					minLevelRange	=	40f, 
					maxLevelRange	=	40, 
					collectableP	=	0.7f, 
					flowerP	=	0.6f,
					level	=	40;

	private bool firstAnimalSpawn = false;


	private void changeParameters(){
		
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
		if(lastTileType.id != Tile.TileType.MovingTile1 && 
			lastTileType.id != Tile.TileType.MovingTile2 &&
			lastTileType.id != Tile.TileType.MovingTile3)
		if(Random.value > 0.3)
			spawnPlatform (false);
		
		level += Random.Range(minLevelRange, maxLevelRange);
		isFirstLiftThisLevel = true;
	}

	float liftP;

	void setTilePercentage(){
		
	}

	 
	void Start(){
		
		//maxXRage = randomRange;
	}

	bool isFirstLiftThisLevel = true;

	void spawnPlatform(bool isFirstTileOfThisLevel){

		Tile tileType = null;
		float randomFloat = Random.value;

		setTilePercentage ();
		float currentP = 0;

		for(int i = 0; i < tilePrefabs.Length && tileType == null; i++){
			
			currentP += tilePrefabs[i].probability;

			if (randomFloat <= currentP) {

				switch(tilePrefabs[i].id){

				case Tile.TileType.MovingTile1:
				case Tile.TileType.MovingTile2:
				case Tile.TileType.MovingTile3:

					if (isFirstTileOfThisLevel)
						tileType = tilePrefabs[i];
					break;

				case Tile.TileType.LiftTile:
					
					if (isFirstLiftThisLevel) {
						tileType = tilePrefabs[i];
						isFirstLiftThisLevel = false;
					}
					break;
					
				default:
					tileType = tilePrefabs[i];
					break;
				}
			}
		}

		if (tileType== null)
			tileType = tilePrefabs[0];

		lastTileType = tileType;

		int platformLength = 1;

		if (!isFirstTileOfThisLevel) {
		
			platformLength = Random.Range (1, 3);

			if (Random.value > 0.5f)
				randomX = lastPlatformPosition + lastPlatformLength * tileWidth + Random.Range (1, 3) * tileWidth;
			else
				randomX = lastPlatformPosition - platformLength * tileWidth - Random.Range (1, 3) * tileWidth;
			
		} else {
			
			if (tileType.id != Tile.TileType.MovingTile1 && 
				tileType.id != Tile.TileType.MovingTile2 &&
				tileType.id != Tile.TileType.MovingTile3 &&
				tileType.id != Tile.TileType.LiftTile)
				platformLength = Random.Range (1, 4);

			float currentRandomX = Random.Range (-randomRange + 1, randomRange);

			if (randomRange - Mathf.Abs (currentRandomX) < 2 && randomRange > 1)
				randomRange -= 1;
			else 
				randomRange = (randomRange < maxXRage) ? randomRange + 1 : maxXRage;

			randomX = lastPlatformPosition + currentRandomX * tileWidth;
			lastPlatformPosition = randomX;
		}

		if (tileType.id == Tile.TileType.LiftTile || 
			tileType.id == Tile.TileType.ColorizeTile || 
			tileType.id == Tile.TileType.BounceTile)
			platformLength = 1;
		
		lastPlatformLength = platformLength;


		Vector2 v2 = new Vector2 (randomX, level);

		for (int i = 0; i < platformLength; i++) {
			
			v2 = new Vector3 (randomX + i * tileWidth, level, 0);

			Instantiate (tileType.tile, v2, Quaternion.identity);

			if(!spawnFlowers (v2, tileType.id))
			if(!spawnCollectable (v2, tileType.id))
				spawnAnimal (v2, tileType.id);
		}

		lastXPosition = v2.x;
	}

	float donutP = 0.9f, pizzaP = 0.05f, hamburgerP = 0.05f;

	private bool spawnCollectable(Vector3 v,  Tile.TileType tileType){
		
		if (tileType != Tile.TileType.LiftTile && 
			tileType != Tile.TileType.SpineTile &&
			tileType != Tile.TileType.MovingTile1 && 
			tileType != Tile.TileType.MovingTile2 &&
			tileType != Tile.TileType.MovingTile3 && 
			tileType != Tile.TileType.ColorizeTile) {

			if (Random.value <= collectableP) {
			
				v.y = v.y + tileWidth;
				v.z = -1;
				float randomP = Random.value;

				if(randomP <= donutP)
					Instantiate (collectablesPrefabs [0], v, Quaternion.identity);
				else if (randomP <= donutP + pizzaP)
					Instantiate (collectablesPrefabs [1], v, Quaternion.identity);
				else if(randomP <= donutP + pizzaP + hamburgerP)
					Instantiate (collectablesPrefabs [2], v, Quaternion.identity);

				return true;
			}
		}

		return false;
	}


	private bool spawnFlowers(Vector3 v, Tile.TileType tileType){


		if (tileType == Tile.TileType.NormalTile && Random.value <= flowerP) {
			
			v.y = v.y + tileWidth / 2;
			v.z = -1;

			Instantiate (flowerPrefab, v, Quaternion.identity);

			return true;
		}

		return false;
	}

	private bool spawnAnimal(Vector3 v,  Tile.TileType tileType){

		if (tileType == Tile.TileType.NormalTile && Random.value <= 0.5f && v.y >= 100) {

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

