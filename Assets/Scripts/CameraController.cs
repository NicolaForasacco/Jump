using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Color[] backgroundColors;
	public bool active = false;
	public float cameraSlowSpeed = 0f,
					increaseSpeedEachTime = 10f,
					landscapeSize = 170f,
					portraitSize = 230f,
					landscapeSizeZoomOn = 160f,
					portraitSizeZoomOn = 180f,
					maxCameraSpeed,
					cameraOffset = 10f,
					smoothDampTime;

	private	GameObject player;
	private	Camera cameraB;
	private	ScreenOrientation previousScreenOrientation;
	private bool isZoomOn = false;
	private	Vector3	offset, newCameraPosition, velocity, initialTransform;

	public void Start () 
	{
		initialSetup ();
	}
		
	void initialSetup(){

		player = GameObject.FindGameObjectWithTag ("player");

		if (player != null) {
			
			cameraB = GetComponent<Camera> ();
			offset = new Vector3 (0, cameraOffset, transform.position.z);
			initialTransform = transform.position;
			newCameraPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);

			if (GameObject.FindObjectOfType<PreferenceManager> ().getRecord () != 0)
				changeRandomBackgroundColor ();
		}
	}
		
	void updateCameraSize(){
		
		if (Screen.orientation == ScreenOrientation.Portrait
		    || Screen.orientation == ScreenOrientation.PortraitUpsideDown) {

			if (!isZoomOn)
				cameraB.orthographicSize = portraitSize;
			else
				cameraB.orthographicSize = portraitSizeZoomOn;

		} else if (Screen.orientation == ScreenOrientation.LandscapeLeft
		           || Screen.orientation == ScreenOrientation.LandscapeRight
		           || Screen.orientation == ScreenOrientation.Landscape
		           || Screen.orientation == ScreenOrientation.AutoRotation) {

			if (!isZoomOn)
				cameraB.orthographicSize = landscapeSize;
			else
				cameraB.orthographicSize = landscapeSizeZoomOn;
		}
	}
		
	void Update(){

		if (previousScreenOrientation != Screen.orientation) {
			
			updateCameraSize ();
			previousScreenOrientation = Screen.orientation;
		}
	}


	void LateUpdate () 
	{
		
		if (active) {
			if (player != null) {
				
				newCameraPosition.x = player.transform.position.x + offset.x;
				newCameraPosition.y = transform.position.y + cameraSlowSpeed * Time.deltaTime;

				if (player.transform.position.y + offset.y >= newCameraPosition.y)
					newCameraPosition.y = player.transform.position.y + offset.y;
				
				newCameraPosition = Vector3.SmoothDamp (transform.position, newCameraPosition, ref velocity,smoothDampTime);

				transform.position = newCameraPosition;
			} else
				initialSetup ();
		}
	}


	public void changeRandomBackgroundColor(){
		
		Color c = backgroundColors [Random.Range (0, backgroundColors.Length)];

		while(c == cameraB.backgroundColor)
			c = backgroundColors [Random.Range (0, backgroundColors.Length)];

		cameraB.backgroundColor = c;
	}

	public void zoomOn(){

		isZoomOn = true;
		updateCameraSize ();

		if (player != null)
			transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
	}

	public void  zoomOff(){

		isZoomOn = false;
		updateCameraSize ();
		transform.position = initialTransform;
	}

	public void increaseSpeed(){

		if (cameraSlowSpeed + increaseSpeedEachTime < maxCameraSpeed)
			cameraSlowSpeed += increaseSpeedEachTime;
		else
			cameraSlowSpeed = maxCameraSpeed;
	}

	public float getXStart(){

		return transform.position.x - Camera.main.orthographicSize * 1.5f;
	}

	public float getXBounds(){

		return transform.position.x + Camera.main.orthographicSize * 1.5f;
	}

	public float getMinPosition(){
		
		return transform.position.y - Camera.main.orthographicSize - 20;
	}

	public float getMaxPosition(){

		return transform.position.y + Camera.main.orthographicSize * 1.5f;
	}
}
