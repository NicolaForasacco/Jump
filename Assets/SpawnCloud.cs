using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCloud : MonoBehaviour {

	public GameObject player;
	public GameObject[] cloudPrefabs;

	float lastPlatformPosition;

	// Use this for initialization
	void Start () {

		lastPlatformPosition = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (player.transform.position.y + 300 > lastPlatformPosition)
			spawnClouds ();
	}

	void spawnClouds(){
		
		float x = player.transform.position.x + Random.Range (-5,5) * 30;
		lastPlatformPosition = lastPlatformPosition + Random.Range (1,5) * 30;

		Vector2 v = new Vector2 (x,lastPlatformPosition);

		Instantiate (cloudPrefabs[Random.Range(0,cloudPrefabs.Length - 1)], v, Quaternion.identity);
	}
}
