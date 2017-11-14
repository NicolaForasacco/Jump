using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyParallax : MonoBehaviour {

	public Transform[] backgrounds;
	public Transform cameraTrasform;
	private CameraController cameraController;

	public float centerPoint;
	private float Width;

	private Vector3 lastCamera;

	public float parallaxSpeedY = 0f;

	public float maxParallaxSpeedY = 0.1f;

	public float parallaxSpeedX = 0.20f;

	public int rightIndex=2, leftIndex=0;

	void Start(){

		cameraController = GameObject.FindObjectOfType<CameraController> ();

		Width = backgrounds[1].GetComponent<SpriteRenderer>().bounds.size.x;

		centerPoint = Width/2;
	}

	// Update is called once per frame
	void Update () {

		if (centerPoint  + Width/2 < cameraTrasform.position.x ) {

			ScrollRight ();

			centerPoint += Width;

		} else if(centerPoint - Width/2> cameraTrasform.position.x ){

			ScrollLeft ();

			centerPoint -= Width;

		}

		if (cameraController.getMinPosition() >= transform.position.y + transform.localScale.y)
			parallaxSpeedX = maxParallaxSpeedY;

		float deltaX = cameraTrasform.position.x - lastCamera.x;
		float deltaY = cameraTrasform.position.y - lastCamera.y;

		transform.position += Vector3.right * deltaX * parallaxSpeedX;
		transform.position += Vector3.down * deltaY * parallaxSpeedY;
		lastCamera = cameraTrasform.position;
	}

	private void ScrollLeft(){
		
		backgrounds[rightIndex].localPosition = new Vector3(backgrounds[leftIndex].localPosition.x - Width,
			backgrounds[rightIndex].localPosition.y, 
			backgrounds[rightIndex].localPosition.z);

		leftIndex = rightIndex;
		rightIndex--;
		if (rightIndex < 0)
			rightIndex = backgrounds.Length - 1;
		
	}

	private void ScrollRight(){
		
		backgrounds[leftIndex].localPosition =  new Vector3(backgrounds[rightIndex].localPosition.x + Width,
			backgrounds[leftIndex].localPosition.y, 
			backgrounds[leftIndex].localPosition.z);
		rightIndex = leftIndex;
		leftIndex++;
		if (leftIndex == backgrounds.Length)
			leftIndex = 0;
	}
}
