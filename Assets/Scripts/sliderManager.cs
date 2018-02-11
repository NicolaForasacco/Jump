using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class sliderManager : MonoBehaviour {

	public	GameObject[]	characters;
	public	GameManager	gm;
	public	CameraController	cameraController;
	public	GameObject	currentCharacter,
						donutsCostLabel;

	public	int	currentSelectedCharacter = 0;


	private	GameObject	newCurrentCharacter;
	private	PlayerMovement	currentPlayerMovement;
	private	Text	donutsCost;
	private	bool	characterSelectionMenuOpen = false;


	public void InstantiateCharacter () {
		
		//donutsCostLabel = GameObject.FindGameObjectWithTag ("DonutsCostLabel");
		donutsCost = GameObject.FindGameObjectWithTag ("DonutsCost").GetComponent<Text> ();
		donutsCostLabel.SetActive (false);

		gm = GameObject.FindObjectOfType<GameManager> ();

		currentSelectedCharacter = PlayerPrefs.GetInt ("CharacterSelected", 0);
		currentCharacter = characters [currentSelectedCharacter];
		newCurrentCharacter = Instantiate (characters [currentSelectedCharacter], new Vector3 (currentCharacter.transform.position.x, 20, 0), Quaternion.identity);

		//DestroyObject (currentCharacter.gameObject);
		cameraController.Start ();

		currentCharacter = newCurrentCharacter;
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
											new Vector3(currentCharacter.transform.position.x, 40, 0), 
											Quaternion.identity);

		DestroyObject (currentCharacter.gameObject);
		cameraController.Start ();

		currentCharacter = newCurrentCharacter;
		currentPlayerMovement = currentCharacter.GetComponentInChildren<PlayerMovement> ();

		if ( !currentCharacter.GetComponent<CostManager> ().hasBeenBought() && currentCharacter.GetComponent<CostManager> ().cost > 0 ) {

			// rendo il personaggio un po' scuro per far capire che deve essere acquistato
			currentCharacter.GetComponentInChildren<SpriteRenderer> ().color = new Color (0.4f, 0.4f, 0.4f, 1f);

			donutsCostLabel.SetActive (true);
			donutsCost.text = currentCharacter.GetComponent<CostManager> ().cost.ToString ();

		} else {
			donutsCostLabel.SetActive (false);
		}
	}

	public void openSelection (){

		characterSelectionMenuOpen = true;
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

	public void close(){
		int oldSelectedCharacter = PlayerPrefs.GetInt ("CharacterSelected", 0);

		if (oldSelectedCharacter != currentSelectedCharacter) {
			currentSelectedCharacter = oldSelectedCharacter;
			spawnNewCurrentCharacter ();
		}
	}
}
