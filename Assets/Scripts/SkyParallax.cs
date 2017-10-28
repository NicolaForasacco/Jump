using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyParallax : MonoBehaviour {

	public Transform[] backgrounds;
	public Transform cameraTrasform;

	private float currentWidth;
	private bool currentBackground = true;
	private float Width;

	private Vector3 lastCamera;

	public float parallaxSpeed = 0.20f;

	void Start(){

		Width = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x*2;
		currentWidth = Width / 2;
	}

	// Update is called once per frame
	void Update () {

		if (currentWidth < cameraTrasform.position.x) {
			if (currentBackground )
				backgrounds [0].localPosition = new Vector3 (backgrounds[0].localPosition.x + Width,
																			backgrounds[0].localPosition.y,
																			backgrounds[0].localPosition.z);
			else
				backgrounds [1].localPosition = new Vector3 (backgrounds[1].localPosition.x + Width,
																backgrounds[1].localPosition.y,
																backgrounds[1].localPosition.z);
			

			currentWidth += Width/2;
			currentBackground = !currentBackground;

		} else if(currentWidth > cameraTrasform.position.x + Width/2){

			if (currentBackground )
				backgrounds [1].localPosition = new Vector3 (backgrounds[1].localPosition.x - Width,
					backgrounds[1].localPosition.y,
					backgrounds[1].localPosition.z);
			else
				backgrounds [0].localPosition = new Vector3 (backgrounds[0].localPosition.x - Width,
					backgrounds[0].localPosition.y,
					backgrounds[0].localPosition.z);


			currentWidth -= Width/2;
			currentBackground = !currentBackground;

		}

		float deltaX = cameraTrasform.position.x - lastCamera.x;
		transform.position += Vector3.right * deltaX * parallaxSpeed;
		lastCamera = cameraTrasform.position;
	}
}
