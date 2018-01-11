using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public Canvas menu;
	public GameObject bird;

	public bool useButtons;
	public Animator cameraAnim;

	public CameraController camera;
	public bool isFirstPlay = true;

	public Animator menuAnim;

	public void gameEnd(){
		menuAnim.SetTrigger ("GameOver");

		camera.active = false;
		cameraAnim.SetTrigger ("CameraZoomOff");
	}


	void Start(){
		
		menuAnim = menu.GetComponent<Animator> ();
		cameraAnim = camera.GetComponent<Animator> ();

		menuAnim.SetTrigger ("MenuOn");
	}

	public void startGame(){
		
		camera.active = true;
		menuAnim.SetTrigger ("ControlsOn");

		Instantiate (bird, new Vector2(bird.transform.position.x, bird.transform.position.y), Quaternion.identity);

		if (!isFirstPlay) {
			SceneManager.LoadScene ("MainScene", LoadSceneMode.Single);
			//cameraAnim.SetTrigger ("CameraZoomOn");
		} else {
			isFirstPlay = false;
		}
	}

	public void changeButtonsUse(){
		useButtons = !useButtons;
	}
}
