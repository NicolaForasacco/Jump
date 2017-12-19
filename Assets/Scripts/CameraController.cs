using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;

	int min;
	Vector3 offset, v;
	Camera cameraB;
	public bool active = false;

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

	Vector3 velocity;

	void LateUpdate () 
	{
		if (active) {
			
			v.x = player.transform.position.x + offset.x;
			v.y = transform.position.y + 10 * Time.deltaTime;

			if (player.transform.position.y + offset.y >= v.y) {

				v.y = player.transform.position.y + offset.y;
				v = Vector3.SmoothDamp (transform.position, v, ref velocity, 0.1f);
			}

			//v.y = (player.transform.position.y + offset.y >= v.y) ? player.transform.position.y + offset.y : v.y;

			transform.position = v;
		}
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
