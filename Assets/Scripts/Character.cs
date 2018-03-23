using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters")]
public class Character : ScriptableObject {
	
	public Sprite sprite, destroyPieceSprite;

	public int id,
				jumpSpeed = 45000,
				sideSpeed = 170,
				slowSideSpeed = 150,
				mass = 60,
				gravityScale = 180,
				cost;

	public bool boughtWithObjects; // se donut o diamanti o qualsiasi altro oggetto

	int donutsPicked;

	public bool buy(){

		PreferenceManager prefM = GameObject.FindObjectOfType<PreferenceManager> ();

		if ( canBeBought() && !prefM.hasBeenBought (id)) {

			prefM.setHasBought (id);
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
				// guardare se può essere sbloccato per altri motivi
		}

		return false;
	}

	public bool hasBeenBought(){

		return GameObject.FindObjectOfType<PreferenceManager> ().hasBeenBought (id);
	}
}
