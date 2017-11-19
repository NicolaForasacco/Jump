using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

	private CameraController cameraController;
	public bool active = false;
	controller controller;
	SpriteRenderer rd;


	// Use this for initialization
	void Start () {
		cameraController = GameObject.FindObjectOfType<CameraController> ();
		controller = this.transform.parent.GetComponentInParent<controller> ();

		rd = GetComponentInChildren<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (this.transform.position.y <= cameraController.getMinPosition())
			active = true;

		if (this.transform.position.y < cameraController.getMinPosition ())
			this.transform.position += Vector3.up * (controller.gameHeight() + rd.bounds.extents.y/2);

		controller.setNewHeight (transform.position.y + rd.bounds.extents.y/2);
	}
}
