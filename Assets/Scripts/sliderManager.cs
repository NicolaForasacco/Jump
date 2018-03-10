using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class sliderManager : MonoBehaviour {

	public	GameObject[]	charactersPrefabs;
	public GameObject[] characters;

	public	GameManager	gm;
	public	CameraController	cameraController;
	public	GameObject	currentCharacter,
						donutsCostLabel,
						playButton;

	public	int	currentSelectedCharacter = 0;


	private	GameObject	newCurrentCharacter;
	private	PlayerMovement	currentPlayerMovement;
	private	Text	donutsCost;
	private	bool	characterSelectionMenuOpen = false;



	public void InstantiateCharacter () {

		characters = new GameObject[charactersPrefabs.Length];

		//donutsCostLabel = GameObject.FindGameObjectWithTag ("DonutsCostLabel");
		donutsCost = GameObject.FindGameObjectWithTag ("DonutsCost").GetComponent<Text> ();
		donutsCostLabel.SetActive (false);

		gm = GameObject.FindObjectOfType<GameManager> ();

		currentSelectedCharacter = PlayerPrefs.GetInt ("CharacterSelected", 0);

		currentCharacter = charactersPrefabs [currentSelectedCharacter];

		newCurrentCharacter = Instantiate (charactersPrefabs [currentSelectedCharacter], new Vector3 (currentCharacter.transform.position.x, 20, 0), Quaternion.identity);
		characters [currentSelectedCharacter] = newCurrentCharacter;


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
		
		if (currentSelectedCharacter < charactersPrefabs.Length - 1) {
		
			currentSelectedCharacter += 1;

			for (int i = 0; i < characters.Length; i++) {
				characters [i].transform.position = new Vector3 (characters [i].transform.position.x - 40, characters [i].transform.position.y, characters [i].transform.position.z);
			}

			spawnNewCurrentCharacter ();
		}
	}

	public void slideCharacterLeft(){

		if (currentSelectedCharacter > 0) {

			currentSelectedCharacter -= 1;

			for (int i = 0; i < characters.Length; i++) {
				characters [i].transform.position = new Vector3 (characters [i].transform.position.x + 40, characters [i].transform.position.y, characters [i].transform.position.z);
			}

			spawnNewCurrentCharacter ();

		}
	}

	void spawnNewCurrentCharacter(){
		
		//newCurrentCharacter = Instantiate (charactersPrefabs[currentSelectedCharacter], new Vector3(currentCharacter.transform.position.x, 40, 0), Quaternion.identity);

		//DestroyObject (currentCharacter.gameObject);
		//cameraController.Start ();

		currentCharacter = characters[currentSelectedCharacter];
		currentPlayerMovement = currentCharacter.GetComponentInChildren<PlayerMovement> ();

		if ( !currentPlayerMovement.ch.hasBeenBought() && currentPlayerMovement.ch.cost > 0 ) {

			// rendo il personaggio un po' scuro per far capire che deve essere acquistato
			currentCharacter.GetComponentInChildren<SpriteRenderer> ().color = new Color (0.4f, 0.4f, 0.4f, 1f);

			donutsCostLabel.SetActive (true);
			donutsCost.text = currentPlayerMovement.ch.cost.ToString ();

			playButton.GetComponent<Image> ().color = (currentPlayerMovement.ch.canBeBought ())? new Color (1f, 1f, 1f, 1f) : new Color (0.4f, 0.4f, 0.4f, 1f);
			

		} else {
			donutsCostLabel.SetActive (false);
			playButton.GetComponent<Image> ().color = new Color (1f, 1f, 1f, 1f);
		}
	}

	public void spawnCharacter(int i, GameObject character){

		characters [i] = character;
		PlayerMovement pm =  character.GetComponent<PlayerMovement> ();

		pm.disableGravity ();

		if ( !pm.ch.hasBeenBought() && pm.ch.cost > 0 ) {

			// rendo il personaggio un po' scuro per far capire che deve essere acquistato
			character.GetComponentInChildren<SpriteRenderer> ().color = new Color (0.4f, 0.4f, 0.4f, 1f);



			/*if ()
				playButton.GetComponentInChildren<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
			else
				playButton.GetComponentInChildren<SpriteRenderer> ().color = new Color (0.4f, 0.4f, 0.4f, 1f);*/


		} else {
			donutsCostLabel.SetActive (false);
		}
	}

	public void openSelection (){

		characterSelectionMenuOpen = true;


		for(int i = 0; i < currentSelectedCharacter; i++)
			spawnCharacter(i, Instantiate (charactersPrefabs [i], new Vector3 (currentCharacter.transform.position.x + (i - currentSelectedCharacter) * 40, 20, 0), Quaternion.identity));

		for(int i = currentSelectedCharacter + 1; i < charactersPrefabs.Length; i++)
			spawnCharacter(i, Instantiate (charactersPrefabs [i], new Vector3 (currentCharacter.transform.position.x + (i - currentSelectedCharacter) * 40, 20, 0), Quaternion.identity));
	

	}

	public void selectCurrentCharacter(){

		if (characterSelectionMenuOpen) {
			
			if (currentPlayerMovement.ch.buy () && !currentPlayerMovement.dead) { // controllo che sia stato acquistato
			
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

		for (int i = 0; i < characters.Length; i++) {
			if (i != currentSelectedCharacter)
				GameObject.Destroy (characters [i]);
		}

		if (oldSelectedCharacter != currentSelectedCharacter) {
			currentSelectedCharacter = oldSelectedCharacter;
			spawnNewCurrentCharacter ();
		}
	}

	public bool existCharacterToBuy(){

		foreach(GameObject g in charactersPrefabs){
			if (g.GetComponent<PlayerMovement> ().ch.canBeBought())
				return true;
		}

		return false;
	}
}
