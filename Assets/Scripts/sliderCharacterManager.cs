using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class sliderCharacterManager : MonoBehaviour {

	public GameObject characterPrefab;
	public Character currentCharacter;
	public	Character[] characters;

	public Text ownedText;
	public	GameManager	gm;
	public	CameraController cameraController;
	public	GameObject currentCharacterInstance, donutsCostLabel, playButton;

	public	int	currentSelectedCharacterId = 1;


	private GameObject[] charactersInstances;
	private	GameObject	newCurrentCharacterInstance;
	private PreferenceManager prefManager;
	private	PlayerMovement	currentPlayerMovement;
	private	Text donutsCost;
	private	bool characterSelectionMenuOpen;
	private const float currentCharacterScale = 2.8f, distanceBetweenCharacters = 40f;


	void Start(){
		prefManager = GameObject.FindObjectOfType<PreferenceManager> ();

		if (prefManager.isFirstTime ())
			characters [0].buy ();
	}

	public void InstantiateCharacter () {

		prefManager = GameObject.FindObjectOfType<PreferenceManager> ();
		charactersInstances = new GameObject[characters.Length];
	
		donutsCost = GameObject.FindGameObjectWithTag ("DonutsCost").GetComponent<Text> ();
		donutsCostLabel.SetActive (false);

		gm = GameObject.FindObjectOfType<GameManager> ();

		currentSelectedCharacterId = prefManager.getLastSelectedCharacter();

		Debug.Log (currentSelectedCharacterId);

		currentCharacter = characters [currentSelectedCharacterId - 1];


		characterPrefab.GetComponent<PlayerMovement> ().ch = currentCharacter;

		newCurrentCharacterInstance = Instantiate (characterPrefab, new Vector3 (0, 20, 0), Quaternion.identity);
		charactersInstances [currentSelectedCharacterId - 1] = newCurrentCharacterInstance;

		cameraController.Start ();

		currentCharacterInstance = newCurrentCharacterInstance;
		currentPlayerMovement = currentCharacterInstance.GetComponentInChildren<PlayerMovement> ();

	}

	#region Player movements

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

	#endregion

	public void slideCharacterRight(){
		
		if (currentSelectedCharacterId < characters.Length) {
		
			currentSelectedCharacterId += 1;

			foreach(GameObject c in charactersInstances){
				c.transform.position = new Vector3 (c.transform.position.x - distanceBetweenCharacters, c.transform.position.y, c.transform.position.z);
				c.transform.localScale = new Vector2 (1, 1);
			}

			spawnNewCurrentCharacter ();
		}
	}

	public void slideCharacterLeft(){

		if (currentSelectedCharacterId > 1) {

			currentSelectedCharacterId -= 1;

			for (int i = 0; i < charactersInstances.Length; i++) {
				charactersInstances [i].transform.position = new Vector3 (charactersInstances [i].transform.position.x + distanceBetweenCharacters, charactersInstances [i].transform.position.y, charactersInstances [i].transform.position.z);
				charactersInstances [i].transform.localScale = new Vector2 (1, 1);
			}

			spawnNewCurrentCharacter ();
		}
	}


	void spawnNewCurrentCharacter(){

		currentCharacterInstance = charactersInstances[currentSelectedCharacterId - 1];
		currentCharacterInstance.transform.localScale = new Vector2 (currentCharacterScale, currentCharacterScale);

		currentPlayerMovement = currentCharacterInstance.GetComponentInChildren<PlayerMovement> ();

		if ( !currentPlayerMovement.ch.hasBeenBought() && currentPlayerMovement.ch.cost > 0 ) {

			// rendo il personaggio un po' scuro per far capire che deve essere acquistato
			currentCharacterInstance.GetComponentInChildren<SpriteRenderer> ().color = new Color (0.4f, 0.4f, 0.4f, 1f);

			donutsCostLabel.SetActive (true);
			donutsCost.text = currentPlayerMovement.ch.cost.ToString ();

			playButton.GetComponent<Image> ().color = (currentPlayerMovement.ch.canBeBought ())? new Color (1f, 1f, 1f, 1f) : new Color (0.4f, 0.4f, 0.4f, 1f);
			

		} else {
			donutsCostLabel.SetActive (false);
			playButton.GetComponent<Image> ().color = new Color (1f, 1f, 1f, 1f);
		}
	}

	public void spawnCharacter(int i, GameObject character){

		charactersInstances [i] = character;
		PlayerMovement pm =  character.GetComponent<PlayerMovement> ();

		pm.SetGravity (false);

		if ( !pm.ch.hasBeenBought() && pm.ch.cost > 0 )
			character.GetComponentInChildren<SpriteRenderer> ().color = new Color (0.4f, 0.4f, 0.4f, 1f);
		else 
			donutsCostLabel.SetActive (false);
	}

	int getTotalNumberOwnedCharacter(){
		
		int sum = 0;

		foreach(Character c in characters)
			sum += (c.hasBeenBought())? 1 : 0;

		return sum;
	}

	public void openSelection (){

		int characterOwned = getTotalNumberOwnedCharacter(), totalCharacter = characters.Length;
		ownedText.text = characterOwned + "/" + totalCharacter;

		characterSelectionMenuOpen = true;
		currentCharacterInstance.transform.localScale = new Vector2 (currentCharacterScale, currentCharacterScale);
		currentPlayerMovement.SetGravity (false);

		for (int i = 0; i < currentSelectedCharacterId - 1; i++) {
			characterPrefab.GetComponent<PlayerMovement> ().ch = characters [i];
			spawnCharacter (i, Instantiate (characterPrefab, new Vector3 (currentCharacterInstance.transform.position.x + (i - currentSelectedCharacterId + 1) * distanceBetweenCharacters, 20, 0), Quaternion.identity));
		}

		for (int i = currentSelectedCharacterId; i < characters.Length; i++) {
			characterPrefab.GetComponent<PlayerMovement> ().ch = characters [i];
			spawnCharacter (i, Instantiate (characterPrefab, new Vector3 (currentCharacterInstance.transform.position.x + (i - currentSelectedCharacterId + 1) * distanceBetweenCharacters, 20, 0), Quaternion.identity));
		}
	}

	public void selectCurrentCharacter(){

		if (characterSelectionMenuOpen) {

			int oldSelectedCharacterId = prefManager.getLastSelectedCharacter();

			if (currentPlayerMovement.ch.buy () && !currentPlayerMovement.dead) { // controllo che sia stato acquistato

				if (oldSelectedCharacterId == currentSelectedCharacterId) {

					currentCharacterInstance.transform.localScale = new Vector2 (1, 1);
					currentPlayerMovement.SetGravity (true);
					gm.closeCharacterSelectionMenu ();
					gm.startGame ();

				} else {
				
					gm.isFirstPlay = false;  // costringo il game manager a ricaricare la scena
					prefManager.setLastSelectedCharacter(currentSelectedCharacterId);
					gm.startGame ();
				}

			} else if (currentPlayerMovement.dead) {

				prefManager.setLastSelectedCharacter(oldSelectedCharacterId);
				gm.restartGame ();
			}
				
		}else if (currentPlayerMovement.dead)
			gm.restartGame ();
		else
			gm.startGame ();	
	}

	public void close(){
		
		int oldSelectedCharacterId = prefManager.getLastSelectedCharacter ();

		for (int i = 0; i < charactersInstances.Length; i++) {
			if (i != oldSelectedCharacterId - 1)
				GameObject.Destroy (charactersInstances [i]);
		}

		if (oldSelectedCharacterId != currentSelectedCharacterId) {
			
			currentSelectedCharacterId = oldSelectedCharacterId;
			spawnNewCurrentCharacter ();
			currentPlayerMovement.transform.position = new Vector3 (0, currentPlayerMovement.transform.position.y, currentPlayerMovement.transform.position.z);

		}

		currentCharacterInstance.transform.localScale = new Vector2 (1, 1);
		currentPlayerMovement.SetGravity (true);
	}

	public bool existCharacterToBuy(){

		foreach(Character g in characters){
			if (g.canBeBought())
				return true;
		}

		return false;
	}
}
