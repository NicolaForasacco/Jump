using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;

	int min;
	Vector3 offset, v;
	Camera cameraB;


	void Start () 
	{
		cameraB = GetComponent<Camera> ();
		offset = transform.position - player.transform.position;

		transform.position = player.transform.position + offset;
		v = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
	}

	public void changeRandomBackgroundColor(){

		cameraB.backgroundColor = Random.ColorHSV();
	}

	void LateUpdate () 
	{
		
		float newPosition = (player.transform.position.y + offset.y >= transform.position.y) ? player.transform.position.y + offset.y : transform.position.y;

		v.x = player.transform.position.x + offset.x;

		if (newPosition > v.y)
			v.y = newPosition;
		else
			v.y = transform.position.y + 10 * Time.deltaTime;

		transform.position = v;
	}


	public float getXStart(){

		return transform.position.x - Camera.main.orthographicSize;
	}

	public float getXBounds(){

		return transform.position.x + Camera.main.orthographicSize;
	}

	public float getMinPosition(){
		
		return transform.position.y - Camera.main.orthographicSize - 20;
	}

	public float getMaxPosition(){

		return transform.position.y + Camera.main.orthographicSize * 1.5f;
	}
}
