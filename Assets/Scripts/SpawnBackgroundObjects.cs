using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBackgroundObjects : MonoBehaviour {

	public GameObject 	cloudPrefab,
						airplanePrefab,
						baloonPrefab;

	public float	cloudP = 0.7f, 
					ballonP = 0.5f,
					airplaneP = 0.5f;

	private GameObject player;
	float lastSpawnPositionY = 0;

	int	tileWidth = 20,
		minXRange = -6,
		maxXRange = 6,
		minYRange = 5,
		maxYRange = 8;

	CameraController cameraController;

	void Start () {
		
		player = GameObject.FindGameObjectWithTag ("player");
		cameraController	= GameObject.FindObjectOfType<CameraController> ();
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

	float getRandomXForPlane(){
		
		return (Random.value >= 0.5f) ? cameraController.getXBounds() : cameraController.getXStart();
	}

	/*float getRandomSideX(){
		
		if(Random.value <= 0.5f)
			return ();
		else
			return (GameObject.FindGameObjectWithTag ("GameController").GetComponent<SpawnTile> ().getAverageX());
	}*/

	float getRandomY(){
		return lastSpawnPositionY + Random.Range (minYRange, maxYRange) * tileWidth;
	}



	void spawnClouds(){
		
		Vector3 randomPosition = new Vector3 (getRandomX(), getRandomY(), 1);

		if (Random.value <= cloudP)
			Instantiate (cloudPrefab, randomPosition, Quaternion.identity);
		else {

			randomPosition = new Vector3 (getRandomX(), getRandomY(), 1);

			if (Random.value <= ballonP)
				Instantiate (baloonPrefab, randomPosition, Quaternion.identity);
			else {

				randomPosition = new Vector3 (getRandomXForPlane(), getRandomY(), 1);
				if(Random.value <= airplaneP)
					Instantiate (airplanePrefab, randomPosition, Quaternion.identity);
			}
		}

		lastSpawnPositionY = randomPosition.y;
	}
}
