using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour {

	public GameObject playerTransform;
	public GameObject prefab;

	private Queue grounds;

	private int t = 2;

	// Use this for initialization
	void Start () {

		grounds = new Queue ();
	}


	public void spawnMultipleGrounds(){

		for (int i = 0; i < 1; i++)
			spawnPlatform ();
	}

	void spawnPlatform(){

		int platformLength = Random.Range (1, 4);


		Vector2 v = new Vector2 (Mathf.RoundToInt (playerTransform.transform.position.x + Random.Range (-5, 5)),
			Mathf.RoundToInt (playerTransform.transform.position.y + 2));

		Instantiate (prefab, v, Quaternion.identity);

		t += 1;

		/*Vector2 v2;
		for (int i = 1; i < platformLength; i++) {

			v2 = new Vector2 (v.x + i, v.y);

			Instantiate (prefab, v2, Quaternion.identity);
		}*/
	}
}

