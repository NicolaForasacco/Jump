using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sliderManager : MonoBehaviour {

	public GameObject[] characters;
	public GameManager gm;
	public CameraController camera;

	public int currentSelectedCharacter = 0;

	public GameObject currentCharacter;

	GameObject newCurrentCharacter;

	PlayerMovement currentPlayerMovement;

	// Use this for initialization
	public void InstantiateCharacter () {

		gm = GameObject.FindObjectOfType<GameManager> ();

		currentSelectedCharacter = PlayerPrefs.GetInt ("CharacterSelected", 0);

		currentCharacter = characters [currentSelectedCharacter];

		currentCharacter = Instantiate (characters [currentSelectedCharacter], 
			new Vector3 (currentCharacter.transform.position.x, currentCharacter.transform.position.y, 0), 
			Quaternion.identity);
		
		currentPlayerMovement = currentCharacter.GetComponentInChildren<PlayerMovement> ();
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

	void spawnNewCurrentCharacter(){
		
		newCurrentCharacter = Instantiate (characters[currentSelectedCharacter], 
											new Vector3(currentCharacter.transform.position.x, currentCharacter.transform.position.y, 0), 
											Quaternion.identity);

		DestroyObject (currentCharacter.gameObject);
		camera.Start ();

		currentCharacter = newCurrentCharacter;
		currentPlayerMovement = currentCharacter.GetComponentInChildren<PlayerMovement> ();
	}

	public void selectCurrentCharacter(){
		
		if (true) { // controllo che sia stato acquistato
			
			// PlayerPrefs.SetInt (currentCharacter.tag, 1); // 1 indica personaggio acquistato 0 non acquistato/disponibile
			int oldSelectedCharacter = PlayerPrefs.GetInt ("CharacterSelected", 0);

			if (oldSelectedCharacter == currentSelectedCharacter) {
				
				gm.closeCharacterSelectionMenu ();
				gm.startGame ();
			}else {
				
				gm.isFirstPlay = false;  // costringo il game manager a ricaricare la scena
				PlayerPrefs.SetInt ("CharacterSelected", currentSelectedCharacter);
				gm.startGame ();
			}

		}
	}

}
