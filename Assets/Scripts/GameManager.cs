using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public Canvas menu;
	public Canvas controls;
	public bool useButtons;
	public Animator menuAnim;
	public Animator cameraAnim;

	public CameraController camera;
	public bool isFirstPlay = true;

	public void gameEnd(){

		menu.gameObject.SetActive (true);
		menuAnim.SetTrigger ("PlayButtonOn");


		controls.gameObject.SetActive (false);
		camera.active = false;
		cameraAnim.SetTrigger ("CameraZoomOff");

		//SceneManager.LoadScene ("MainScene",LoadSceneMode.Single);
	}


	void Start(){
		
		menuAnim = menu.GetComponent<Animator> ();
		cameraAnim = camera.GetComponent<Animator> ();

		menu.gameObject.SetActive (true);
		menuAnim.SetTrigger ("PlayButtonOn");
	}

	public void startGame(){

		menu.gameObject.SetActive (false);
		camera.active = true;

		if (!isFirstPlay) {
			SceneManager.LoadScene ("MainScene", LoadSceneMode.Single);
			//cameraAnim.SetTrigger ("CameraZoomOn");
		} else {
		
			isFirstPlay = false;

			if (useButtons)
				controls.gameObject.SetActive (true);
		}
	}

	public void changeButtonsUse(){
		useButtons = !useButtons;
	}
}
