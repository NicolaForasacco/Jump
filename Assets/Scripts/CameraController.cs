using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private GameObject player;

	int min;
	Vector3 offset, v, velocity;
	Camera cameraB;
	public bool active = false;
	public float cameraSlowSpeed = 70;

	public void Start () 
	{
		initialSetup ();

	}

	ScreenOrientation previousScreenOrientation;

	void initialSetup(){

		player = GameObject.FindGameObjectWithTag ("player");

		if (player != null) {
			
			cameraB = GetComponent<Camera> ();
			offset = transform.position - player.transform.position;

			transform.position = player.transform.position + offset;
			v = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		}
	}

	public void changeRandomBackgroundColor(){

		cameraB.backgroundColor = Random.ColorHSV();
	}
	public int minSize = 20,maxSize = 180;

	void Update(){

		if (previousScreenOrientation != Screen.orientation || previousScreenOrientation == null) {
			
			Debug.Log (Screen.orientation);

			if (Screen.orientation == ScreenOrientation.Portrait
			    || Screen.orientation == ScreenOrientation.PortraitUpsideDown) {

				cameraB.orthographic = true;
				cameraB.orthographicSize = maxSize;
				Debug.Log (cameraB.orthographicSize);

			} else if (Screen.orientation == ScreenOrientation.LandscapeLeft
			          || Screen.orientation == ScreenOrientation.LandscapeRight
			          || Screen.orientation == ScreenOrientation.Landscape) {

				cameraB.orthographic = true;
				cameraB.orthographicSize = minSize;

			}else if (Screen.orientation == ScreenOrientation.AutoRotation) {

				cameraB.orthographic = true;
				cameraB.orthographicSize = minSize;

			} else {
				
				Debug.Log (Screen.orientation);
			}

			previousScreenOrientation = Screen.orientation;
		}
	}
		
	void LateUpdate () 
	{
		if (active) {
			
			if (player != null) {
				
				v.x = player.transform.position.x + offset.x;
				v.y = transform.position.y + cameraSlowSpeed * Time.deltaTime;

				if (player.transform.position.y + offset.y >= v.y) {

					v.y = player.transform.position.y + offset.y;

				}

				v = Vector3.SmoothDamp (transform.position, v, ref velocity, 0.1f);

				//v.y = (player.transform.position.y + offset.y >= v.y) ? player.transform.position.y + offset.y : v.y;

				transform.position = v;
			} else {
				initialSetup ();
			}
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
