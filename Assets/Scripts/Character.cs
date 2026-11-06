using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters")]
public class Character : ScriptableObject {
	
	public Sprite sprite;

	public int id, jumpSpeed = 0, sideSpeed, slowSideSpeed, mass, gravityScale, cost;

	public bool boughtWithObjects; // whether using donuts, diamonds or any other item
	public bool differentProperties = false;

	int donutsPicked;

	public bool buy(){

		PreferenceManager prefM = GameObject.FindObjectOfType<PreferenceManager> ();

		if ( canBeBought() && !prefM.hasBeenBought (id)) {

			prefM.setHasBought (id);
			prefM.setLastSelectedCharacter (id);

			prefM.setDonut (prefM.getDonut() - cost);

			return true;

		} else if (prefM.hasBeenBought(id))
			return true;
		else
			return false;
	}

	public bool canBeBought(){

		if (boughtWithObjects && cost <= GameObject.FindObjectOfType<PreferenceManager> ().getDonut() && !hasBeenBought())
			return true;
		else {
				// check if it can be unlocked for other reasons
		}

		return false;
	}

	public bool hasBeenBought(){

		return GameObject.FindObjectOfType<PreferenceManager> ().hasBeenBought (id);
	}
}
