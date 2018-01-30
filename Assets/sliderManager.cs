using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sliderManager : MonoBehaviour {

	public GameObject[] characters;
	public GameManager gm;
	public CameraController camera;

	int currentSelectedCharacter = 0;

	public GameObject currentCharacter;

	GameObject newCurrentCharacter;

	PlayerMovement currentPlayerMovement;

	// Use this for initialization
	void Start () {

		gm = GameObject.FindObjectOfType<GameManager> ();
		currentSelectedCharacter = 0;//PlayerPrefs.GetInt ("CharacterSelected", 0);

		currentCharacter = characters [0];

		/*currentCharacter = Instantiate (characters[currentSelectedCharacter], 
			new Vector3(currentCharacter.transform.position.x, 120, 0), 
			Quaternion.identity);*/
		
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
		
		if (currentSelectedCharacter > 0) {


			currentSelectedCharacter -= 1;

			newCurrentCharacter = Instantiate (characters[currentSelectedCharacter], 
											new Vector3(currentCharacter.transform.position.x, 120, 0), 
											Quaternion.identity);
			
			DestroyObject (currentCharacter.gameObject);
			camera.Start ();

			currentCharacter = newCurrentCharacter;
			currentPlayerMovement = currentCharacter.GetComponentInChildren<PlayerMovement> ();
		}
	}

	public void slideCharacterLeft(){

		if (currentSelectedCharacter < characters.Length - 1) {

			currentSelectedCharacter += 1;


			newCurrentCharacter = Instantiate (characters[currentSelectedCharacter], 
												new Vector3(currentCharacter.transform.position.x, 120, 0), 
												Quaternion.identity);

			DestroyObject (currentCharacter.gameObject);
			camera.Start ();

			currentCharacter = newCurrentCharacter;
			currentPlayerMovement = currentCharacter.GetComponentInChildren<PlayerMovement> ();


		}
	}

	public void selectCurrentCharacter(){
		
		if (true) { // controllo che sia stato acquistato
			
			// PlayerPrefs.SetInt (currentCharacter.tag, 1); // 1 indica personaggio acquistato 0 non acquistato/disponibile
			int oldSelectedCharacter = 0;//PlayerPrefs.GetInt ("CharacterSelected", 0);

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
