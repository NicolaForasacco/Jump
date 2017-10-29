using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMoving : MonoBehaviour {

	public GameObject player;

	Vector3 offset;
	Vector3 v;

	// Use this for initialization
	void Start () {


		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		offset = transform.position - player.transform.position;

		transform.position = player.transform.position + offset;
		v = new Vector3 (transform.position.x,transform.position.y,transform.position.z);
	}
	
	// Update is called once per frame
	void LateUpdate () {


		float newPosition = (player.transform.position.y + offset.y >= transform.position.y) ? player.transform.position.y + offset.y : transform.position.y;

		v.x = player.transform.position.x + offset.x;
		v.y = newPosition;

		transform.position = v;
	}
}
