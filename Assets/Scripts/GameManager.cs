using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public Canvas menu;
	public Canvas controls;
	public bool useButtons;

	public void gameEnd(){

		menu.gameObject.SetActive (true);
		controls.gameObject.SetActive (false);

		SceneManager.LoadScene ("MainScene",LoadSceneMode.Single);
	}

	public void startGame(){
		menu.gameObject.SetActive (false);

		if(useButtons)
			controls.gameObject.SetActive (true);

	}

	public void changeButtonsUse(){
		useButtons = !useButtons;
	}
}
