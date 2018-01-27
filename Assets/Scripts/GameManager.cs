using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Canvas menu;

	public bool useButtons;
	public Animator cameraAnim;

	public CameraController camera;
	public bool isFirstPlay = true;

	public Animator menuAnim;
	public Text recordText;


	public void gameEnd(){

		int record = PlayerPrefs.GetInt ("Record", 0);
		int donuts = PlayerPrefs.GetInt ("Donuts", 0);

		donuts += player.donutsPicked;
		PlayerPrefs.SetInt ("Donuts", donuts);

		recordText.text = "Best " + record.ToString ();

		menuAnim.SetTrigger ("GameOver");

		camera.active = false;
		cameraAnim.SetTrigger ("CameraZoomOff");



		if (player.currentRecord > record) {
			PlayerPrefs.SetInt ("Record", player.currentRecord);
			record = player.currentRecord;
		}
	}

	PlayerMovement player;

	void Start(){
		
		player = GameObject.FindGameObjectWithTag ("player").GetComponent<PlayerMovement>();

		menuAnim = menu.GetComponent<Animator> ();
		cameraAnim = camera.GetComponent<Animator> ();

		menuAnim.SetTrigger ("MenuOn");
	}

	public void startGame(){
		
		camera.active = true;
		menuAnim.SetTrigger ("ControlsOn");

		//Instantiate (bird, new Vector2(bird.transform.position.x, bird.transform.position.y), Quaternion.identity);

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
