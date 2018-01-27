using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sliderManager : MonoBehaviour {

	public GameObject[] characters;

	int currentSelectedCharacter = 0;

	public GameObject currentCharacter;

	GameObject newCurrentCharacter;

	PlayerMovement currentPlayerMovement;

	// Use this for initialization
	void Start () {
		
		currentSelectedCharacter = 0;

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

			currentCharacter = newCurrentCharacter;
			currentPlayerMovement = currentCharacter.GetComponentInChildren<PlayerMovement> ();
		}
	}

}
