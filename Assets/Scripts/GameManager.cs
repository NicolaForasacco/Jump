using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Canvas menu;

	public bool useButtons;
	public Animator cameraAnim;

	public CameraController cameraController;
	public bool isFirstPlay = true;

	public Animator menuAnim;
	public Text recordText;

	public GameObject foreground;

	public void gameEnd(){

		int record = PlayerPrefs.GetInt ("Record", 0);

		PlayerPrefs.SetInt ("Donuts", player.donutsPicked);

		recordText.text = "Best " + record.ToString ();

		menuAnim.SetTrigger ("GameOver");

		cameraController.active = false;
		cameraAnim.SetTrigger ("CameraZoomOff");



		if (player.currentRecord > record) {
			PlayerPrefs.SetInt ("Record", player.currentRecord);
			record = player.currentRecord;
		}
	}

	PlayerMovement player;
	public sliderManager sm;

	void Start(){

		//int characterSelected = PlayerPrefs.GetInt ("CharacterSelected", 0);
		sm.InstantiateCharacter();

		player = GameObject.FindGameObjectWithTag ("player").GetComponent<PlayerMovement>();

		menuAnim = menu.GetComponent<Animator> ();
		cameraAnim = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Animator> ();

		menuAnim.SetTrigger ("MenuOn");

		//deleteAll ();
	}

	void deleteAll(){
		
		PlayerPrefs.DeleteAll ();
	}

	public void startGame(){

		if (!isFirstPlay) {
			SceneManager.LoadScene ("MainScene", LoadSceneMode.Single);
		} else {
			isFirstPlay = false;
			cameraController.active = true;
			foreground.SetActive (true);
			menuAnim.SetTrigger ("ControlsOn");

			if(player!= null)
				player.activate ();
			else
				player = GameObject.FindGameObjectWithTag ("player").GetComponent<PlayerMovement>();

			if(player!= null)
				player.activate ();
		}
	}

	public void showCharacterSelectionMenu(){
		
		sm.openSelection ();
		menuAnim.SetTrigger ("CharacterSelectionMenu");
		cameraAnim.SetTrigger ("CameraZoomCharacter");
		foreground.SetActive (false);
	}

	public void closeCharacterSelectionMenu(){

		//menuAnim.SetTrigger ("MenuOn");
		cameraAnim.SetTrigger ("CameraZoomOn");
		foreground.SetActive (true);
	}

	public void closeCharacterSelectionMenuShowMenu(){

		menuAnim.SetTrigger ("MenuOn");
		cameraAnim.SetTrigger ("CameraZoomOn");
		foreground.SetActive (true);
	}


	public void changeButtonsUse(){
		useButtons = !useButtons;
	}
}
