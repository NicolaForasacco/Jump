using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCloud : MonoBehaviour {

	private GameObject player;
	public GameObject[] cloudPrefabs;

	float lastPlatformPositionY,
			lastPlatformPositionX;

	void Start () {
		
		player = GameObject.FindGameObjectWithTag ("player");
		lastPlatformPositionY = 0;
	}

	void Update () {
		
		if (player != null) {
			
			if (player.transform.position.y + 300 > lastPlatformPositionY)
				spawnClouds ();
		}else
			player = GameObject.FindGameObjectWithTag ("player");
	}


	void spawnClouds(){
		
		lastPlatformPositionX = player.transform.position.x + Random.Range (-5, 5) * 30;
		lastPlatformPositionY = lastPlatformPositionY + Random.Range (1, 4) * 30;

		Vector3 v = new Vector3 (lastPlatformPositionX, 
									lastPlatformPositionY, 
									1);

		int cloudType = (Random.value <= 0.95f) ? Random.Range (0, cloudPrefabs.Length - 1) : cloudPrefabs.Length - 1;

		Instantiate (cloudPrefabs[cloudType], v, Quaternion.identity);
	}
}
