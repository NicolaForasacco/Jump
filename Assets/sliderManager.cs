using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class sliderManager : MonoBehaviour {

	public GameObject[] characters;
	public GameManager gm;
	public CameraController cameraController;

	public int currentSelectedCharacter = 0;

	public GameObject currentCharacter;

	GameObject newCurrentCharacter;

	PlayerMovement currentPlayerMovement;

	public GameObject donutsCostLabel;

	bool characterSelectionMenuOpen = false;

	public void openSelection (){
		
		characterSelectionMenuOpen = true;
	}

	// Use this for initialization
	public void InstantiateCharacter () {
		
		//donutsCostLabel = GameObject.FindGameObjectWithTag ("DonutsCostLabel");
		donutsCost = GameObject.FindGameObjectWithTag ("DonutsCost").GetComponent<Text> ();

		gm = GameObject.FindObjectOfType<GameManager> ();

		currentSelectedCharacter = PlayerPrefs.GetInt ("CharacterSelected", 0);

		currentCharacter = characters [currentSelectedCharacter];

		currentCharacter = Instantiate (characters [currentSelectedCharacter], 
			new Vector3 (currentCharacter.transform.position.x, currentCharacter.transform.position.y, 0), 
			Quaternion.identity);
		
		currentPlayerMovement = currentCharacter.GetComponentInChildren<PlayerMovement> ();

		if ( !currentCharacter.GetComponent<CostManager> ().hasBeenBought()) {

			donutsCostLabel.SetActive (true);
			donutsCost.text = currentCharacter.GetComponent<CostManager> ().cost.ToString ();

		} else {

			donutsCostLabel.SetActive (false);
			donutsCost.text = "";
		}
	}

	public void goLeft(){

		if (currentPlayerMovement != null)
			currentPlayerMovement.goLeft ();
	}

	public void goRight(){

		if (currentPlayerMovement != null)
			currentPlayerMovement.goRight();
	}

	public void stop(){

		if (currentPlayerMovement != null)
			currentPlayerMovement.stopHorizontalSpeed();
	}

	public void jumpDown(){

		if (currentPlayerMovement != null)
			currentPlayerMovement.onJumpButtonDown();
	}

	public void jumpUp(){

		if (currentPlayerMovement != null)
			currentPlayerMovement.onJumpButtonUp();
	}

	// Update is called once per frame
	void Update () {}

	public void slideCharacterRight(){
		
		if (currentSelectedCharacter < characters.Length - 1) {
		
			currentSelectedCharacter += 1;
			spawnNewCurrentCharacter ();
		}
	}

	public void slideCharacterLeft(){

		if (currentSelectedCharacter > 0) {

			currentSelectedCharacter -= 1;
			spawnNewCurrentCharacter ();
		}
	}

	Text donutsCost;

	void spawnNewCurrentCharacter(){
		
		newCurrentCharacter = Instantiate (characters[currentSelectedCharacter], 
											new Vector3(currentCharacter.transform.position.x, currentCharacter.transform.position.y, 0), 
											Quaternion.identity);

		DestroyObject (currentCharacter.gameObject);
		cameraController.Start ();

		currentCharacter = newCurrentCharacter;
		currentPlayerMovement = currentCharacter.GetComponentInChildren<PlayerMovement> ();

		if ( !currentCharacter.GetComponent<CostManager> ().hasBeenBought() && currentCharacter.GetComponent<CostManager> ().cost > 0 ) {
			
			donutsCostLabel.SetActive (true);
			donutsCost.text = currentCharacter.GetComponent<CostManager> ().cost.ToString ();

		} else {
			
			donutsCostLabel.SetActive (false);
			donutsCost.text = "";
		}
	}


	public void selectCurrentCharacter(){

		if (characterSelectionMenuOpen) {
			
			if (currentCharacter.GetComponent<CostManager> ().buy () && !currentPlayerMovement.dead) { // controllo che sia stato acquistato
			
				int oldSelectedCharacter = PlayerPrefs.GetInt ("CharacterSelected", 0);

				if (oldSelectedCharacter == currentSelectedCharacter) {
				
					gm.closeCharacterSelectionMenu ();
					gm.startGame ();
				} else {
				
					gm.isFirstPlay = false;  // costringo il game manager a ricaricare la scena
					PlayerPrefs.SetInt ("CharacterSelected", currentSelectedCharacter);
					gm.startGame ();
				}
			} else {
				gm.isFirstPlay = false;  // costringo il game manager a ricaricare la scena
				PlayerPrefs.SetInt ("CharacterSelected", currentSelectedCharacter);
				gm.startGame ();
			}
		}else
			gm.startGame ();
			
	}

}
