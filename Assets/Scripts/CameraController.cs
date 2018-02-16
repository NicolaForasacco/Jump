using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


	public Color[] backgroundColors;
	public	bool	active	=	false;
	public	float	cameraSlowSpeed	=	70,
					landscapeSize	= 	120f,
					portraitSize	= 	180f,
					landscapeSizeZoomOn	= 60f,
					portraitSizeZoomOn	= 90f;


	private	GameObject player;
	private	Camera cameraB;
	private	ScreenOrientation previousScreenOrientation;
	private	Vector3	offset, newCameraPosition, velocity;
	private int currentColorIndex = 0;


	public void Start () 
	{
		initialSetup ();
	}

	void initialSetup(){

		player = GameObject.FindGameObjectWithTag ("player");

		if (player != null) {
			
			cameraB = GetComponent<Camera> ();
			offset = transform.position - player.transform.position;

			transform.position = player.transform.position + offset;
			newCameraPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		}
	}



	public void changeRandomBackgroundColor(){
		
		currentColorIndex++;
		if (currentColorIndex == backgroundColors.Length)
			currentColorIndex = 0;
		
		cameraB.backgroundColor = backgroundColors[currentColorIndex];

	}

	private bool isZoomOn = false;

	public void zoomOn(){
		
		isZoomOn = true;
		updateCameraSize ();
	}

	public void  zoomOff(){
		
		isZoomOn = false;
		updateCameraSize ();
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

		if (previousScreenOrientation != Screen.orientation || previousScreenOrientation == null) {
			
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

				if (player.transform.position.y + offset.y >= newCameraPosition.y) {

					newCameraPosition.y = player.transform.position.y + offset.y;

				}

				newCameraPosition = Vector3.SmoothDamp (transform.position, newCameraPosition, ref velocity, 0.1f);

				//v.y = (player.transform.position.y + offset.y >= v.y) ? player.transform.position.y + offset.y : v.y;

				transform.position = newCameraPosition;
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
