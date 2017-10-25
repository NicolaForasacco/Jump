using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour {

	public GameObject player;
	public GameObject[] prefab;

	private Queue grounds;
	public int maxPlatformLength = 3;
	public int maxPlatformPerLevel = 3;


	float lastPlatformPosition;
	private int t = 2;

	void Start () {
		
		grounds = new Queue ();
		lastPlatformPosition = 0;
		lastPlatformPosition = player.transform.position.x;
	}

	public void spawnMultiplePlatform(){
		
		Debug.Log ("spawnMultiplePlatform");
		for (int i = 0; i < 60; i++)
			spawnALevelOfPlatforms ();
	}

	public void spawnALevelOfPlatforms(){

		for (int i = 0; i < maxPlatformPerLevel; i++) 
			spawnPlatform ();
		
		t += 2;
	}


	void spawnPlatform(){

		int platformLength = Random.Range (1, maxPlatformLength);

		int randomRange = platformLength * 3;

		int randomX = Mathf.RoundToInt (lastPlatformPosition + Random.Range (- randomRange + 1, randomRange));
		int randomY = Mathf.RoundToInt (t);

		lastPlatformPosition = randomX;

		Vector2 v = new Vector2 (randomX, randomY);

		Instantiate (prefab[Random.Range(0,prefab.Length)], v, Quaternion.identity);

		Vector2 v2;
		for (int i = 1; i < platformLength; i++) {

			v2 = new Vector2 (v.x + i, v.y);

			Instantiate (prefab[Random.Range(0,prefab.Length)], v2, Quaternion.identity);
		}
	}
}

