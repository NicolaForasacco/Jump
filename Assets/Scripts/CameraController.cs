using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private int min;
	public GameObject player;       //Public variable to store a reference to the player game object


	private Vector3 offset;         //Private variable to store the offset distance between the player and camera
	private Vector3 v;



	// Use this for initialization
	void Start () 
	{
		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		offset = transform.position - player.transform.position;

		transform.position = player.transform.position + offset;
		v = new Vector3 (transform.position.x,transform.position.y,transform.position.z);
	}

	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.

		float newPosition = (player.transform.position.y + offset.y >= transform.position.y) ? player.transform.position.y + offset.y : transform.position.y;

		v.x = player.transform.position.x + offset.x;
		v.y = newPosition;

		transform.position = v;
	}


	public float getXStart(){

		return transform.position.x - Camera.main.orthographicSize * 2;
	}

	public float getYBounds(){

		return transform.position.x + Camera.main.orthographicSize * 2;
	}

	public float getMinPosition(){
		
		//Debug.Log (transform.position.y - Camera.main.orthographicSize * 2);
		return transform.position.y - Camera.main.orthographicSize * 2;
	}
}
