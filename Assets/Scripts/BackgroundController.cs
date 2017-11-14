using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

	private CameraController cameraController;

	// Use this for initialization
	void Start () {
		cameraController = GameObject.FindObjectOfType<CameraController> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (this.transform.position.y + 10 < cameraController.getMinPosition ())
			Destroy (this.gameObject);
	}
}
