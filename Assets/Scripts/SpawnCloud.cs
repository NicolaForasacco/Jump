using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCloud : MonoBehaviour {

	public GameObject player;
	public GameObject[] cloudPrefabs;

	float lastPlatformPositionY,
			lastPlatformPositionX;

	// Use this for initialization
	void Start () {

		lastPlatformPositionY = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (player.transform.position.y + 300 > lastPlatformPositionY)
			spawnClouds ();
	}


	void spawnClouds(){
		
		lastPlatformPositionX = player.transform.position.x + Random.Range (-5,5) * 30;
		lastPlatformPositionY = lastPlatformPositionY + Random.Range (1,4) * 30;

		Vector3 v = new Vector3 (lastPlatformPositionX, lastPlatformPositionY, 1);

		Instantiate (cloudPrefabs[Random.Range(0,cloudPrefabs.Length)], v, Quaternion.identity);
	}
}
