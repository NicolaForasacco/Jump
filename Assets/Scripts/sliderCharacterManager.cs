using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sliderCharacterManager : MonoBehaviour {

	public GameObject characterPrefab;
	public Character currentCharacter;
	public Character[] characters;
	public Text ownedText;
	public Text currentCharacterText;
	public GameManager gm;
	public CameraController cameraController;
	public GameObject currentCharacterInstance, donutsCostLabel, playButton;


	private GameObject[] charactersInstances;
	private	GameObject	newCurrentCharacterInstance;
	private PreferenceManager prefManager;
	private	PlayerMovement	currentPlayerMovement;
	private	Text donutsCost;
	private	bool characterSelectionMenuOpen;
	private const float currentCharacterScale = 2.8f, distanceBetweenCharacters = 40f, characterSize = 20f;
	private int numberOfCharacterPerSideSpawned = 7, currentCharacterIndex = 0, currentSelectedCharacterId = 1;

	void Start(){
		prefManager = GameObject.FindObjectOfType<PreferenceManager> ();
		buyInitialCharacter ();
	}

	public void buyInitialCharacter(){
		
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

		currentCharacterIndex = findCharacterIndexFromId (currentSelectedCharacterId);

		currentCharacter = characters [currentCharacterIndex];


		characterPrefab.GetComponent<PlayerMovement> ().ch = currentCharacter;

		newCurrentCharacterInstance = Instantiate (characterPrefab, new Vector3 (0, characterSize, 0), Quaternion.identity);
		charactersInstances [currentCharacterIndex] = newCurrentCharacterInstance;

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

			int l = currentCharacterIndex;
			while(l < characters.Length){
				
				if(l < currentCharacterIndex + 1 + numberOfCharacterPerSideSpawned && charactersInstances[l] == null){

					characterPrefab.GetComponent<PlayerMovement> ().ch = characters [l];
					spawnCharacter (l, Instantiate (characterPrefab, new Vector3 (currentCharacterInstance.transform.position.x + (l - currentCharacterIndex) * distanceBetweenCharacters, characterSize, 0), Quaternion.identity));
				}
				l++;
			}
				
			currentCharacterIndex++;
			currentSelectedCharacterId = findCharacterIdFromIndex (currentCharacterIndex);

			printCurrentCharacterNumber ();

			changeCharactersPosition (-1);

			spawnNewCurrentCharacter ();
		}
	}

	public void slideCharacterLeft(){

		if (currentSelectedCharacterId > 1) {

			int i = currentCharacterIndex - 1;

			while(i >= 0){

				if(i >= currentCharacterIndex - 2 - numberOfCharacterPerSideSpawned && charactersInstances [i] == null) {
						
					characterPrefab.GetComponent<PlayerMovement> ().ch = characters [i];
					spawnCharacter (i, Instantiate (characterPrefab, new Vector3 (currentCharacterInstance.transform.position.x + (i - currentCharacterIndex) * distanceBetweenCharacters, characterSize, 0), Quaternion.identity));
				}
				i--;
			}

			currentCharacterIndex--;
			currentSelectedCharacterId = findCharacterIdFromIndex (currentCharacterIndex);

			printCurrentCharacterNumber ();

			changeCharactersPosition (1);

			spawnNewCurrentCharacter ();
		}
	}
		

	private void changeCharactersPosition(int offset){

		foreach(GameObject c in charactersInstances)
			if (c != null) {
				c.transform.position = new Vector3 (c.transform.position.x + offset * distanceBetweenCharacters, c.transform.position.y, c.transform.position.z);
				c.transform.localScale = new Vector2 (1, 1);
			} 
	}

	void spawnNewCurrentCharacter(){

		currentCharacterInstance = charactersInstances[findCharacterIndexFromId (currentSelectedCharacterId)];
		currentCharacterInstance.transform.localScale = new Vector2 (currentCharacterScale, currentCharacterScale);

		currentPlayerMovement = currentCharacterInstance.GetComponentInChildren<PlayerMovement> ();

		if ( !currentPlayerMovement.ch.hasBeenBought() && currentPlayerMovement.ch.cost > 0 ) {

			// darken the character to indicate that it needs to be purchased
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

		if ( !pm.ch.hasBeenBought() && pm.ch.cost > 0 )
			character.GetComponentInChildren<SpriteRenderer> ().color = new Color (0.4f, 0.4f, 0.4f, 1f);
		else 
			donutsCostLabel.SetActive (false);
	}

	public void openSelection (){

		int characterOwned = getTotalNumberOwnedCharacter(), totalCharacter = characters.Length;
		ownedText.text = characterOwned + "/" + totalCharacter;
		printCurrentCharacterNumber ();

		characterSelectionMenuOpen = true;
		currentCharacterInstance.transform.localScale = new Vector2 (currentCharacterScale, currentCharacterScale);

		int i = currentCharacterIndex - 1, l = currentCharacterIndex + 1;

		while(i >= 0 || l < characters.Length){

			if(i >= currentCharacterIndex - 1 - numberOfCharacterPerSideSpawned  && i >= 0){
				
				characterPrefab.GetComponent<PlayerMovement> ().ch = characters [i];
				spawnCharacter (i, Instantiate (characterPrefab, new Vector3 (currentCharacterInstance.transform.position.x + (i - currentCharacterIndex) * distanceBetweenCharacters, characterSize, 0), Quaternion.identity));
			}
				
			if(l <= currentCharacterIndex + 1 + numberOfCharacterPerSideSpawned && l < characters.Length){

				characterPrefab.GetComponent<PlayerMovement> ().ch = characters [l];
				spawnCharacter (l, Instantiate (characterPrefab, new Vector3 (currentCharacterInstance.transform.position.x + (l - currentCharacterIndex) * distanceBetweenCharacters, characterSize, 0), Quaternion.identity));
			}

			i--;
			l++;
		}
	}

	public void selectCurrentCharacter(){

		if (characterSelectionMenuOpen) {

			int oldSelectedCharacterId = prefManager.getLastSelectedCharacter();

			if (currentPlayerMovement.ch.buy () && !currentPlayerMovement.dead) { // check that it has been purchased

				if (oldSelectedCharacterId == currentSelectedCharacterId) {

					characterSelectionMenuOpen = false;
					gm.closeCharacterSelectionMenuAndShowMenu ();

					/*
					currentCharacterInstance.transform.localScale = new Vector2 (1, 1);
					currentPlayerMovement.SetGravity (true);
					gm.closeCharacterSelectionMenu ();
					gm.startGame ();
						*/
				} else {
				
					gm.isFirstPlay = false;  // force the game manager to reload the scene
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
			if (characters[i].id != oldSelectedCharacterId)
				GameObject.Destroy (charactersInstances [i]);
		}

		if (oldSelectedCharacterId != currentSelectedCharacterId) {
			
			currentSelectedCharacterId = oldSelectedCharacterId;
			spawnNewCurrentCharacter ();
			currentPlayerMovement.transform.position = new Vector3 (0, currentPlayerMovement.transform.position.y, currentPlayerMovement.transform.position.z);
		}

		currentCharacterInstance.transform.localScale = new Vector2 (1, 1);
		characterSelectionMenuOpen = false;
	}


	public bool existCharacterToBuy(){

		foreach(Character g in characters){
			if (g.canBeBought())
				return true;
		}

		return false;
	}

	private void printCurrentCharacterNumber(){

		currentCharacterText.text = (currentCharacterIndex + 1) + "/" + characters.Length;
	}

	int getTotalNumberOwnedCharacter(){

		int sum = 0;

		foreach(Character c in characters)
			sum += (c.hasBeenBought())? 1 : 0;

		return sum;
	}

	private int findCharacterIndexFromId(int id){

		for (int i = 0; i < characters.Length; i++)
			if (characters [i].id == id)
				return i;

		return -1;
	}

	private int findCharacterIdFromIndex(int index){
		return characters[index].id;
	}
}
