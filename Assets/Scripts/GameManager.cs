using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public Canvas menu;
	public Canvas controls;

	public void gameEnd(){
		SceneManager.LoadScene ("MainScene",LoadSceneMode.Single);
	}

	public void startGame(){
		menu.gameObject.SetActive (false);
		controls.gameObject.SetActive (true);
	}
}
