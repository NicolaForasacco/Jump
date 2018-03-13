using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters")]
public class Character : ScriptableObject {
	
	public Sprite sprite;

	public int id,
				jumpSpeed = 45000,
				sideSpeed = 170,
				slowSideSpeed = 150,
				mass = 60,
				gravityScale = 180,
				cost;

	public objectType type;
	public bool boughtWithObjects; // se donut o diamanti o qualsiasi altro oggetto

	public enum objectType{donut, pizza, hamburger};

	public int getCount(){
		return cost;
	}

	public string getType(){

		switch(type){
		case objectType.donut:
			return "Donuts";

		case objectType.pizza:
			return "Pizza";

		case objectType.hamburger:
			return "Hamburger";

		default:
			return "";
		}

	}

	int donutsPicked;

	public bool buy(){



		if ( canBeBought() && PlayerPrefs.GetInt ("Isbought" + id, 0) == 0) {

			PlayerPrefs.SetInt("Isbought" + id, 1);

			int objectsPicked = PlayerPrefs.GetInt (getType(), 0);
			objectsPicked -= cost;

			PlayerPrefs.SetInt (getType(), objectsPicked);

			return true;
		} else if (PlayerPrefs.GetInt ("Isbought" + id, 0) == 1)
			return true;
		else
			return false;

	}

	public bool canBeBought(){

		if (boughtWithObjects && cost <= PlayerPrefs.GetInt (getType(), 0) && !hasBeenBought())
			return true;
		else {
				// guardare se può essere sbloccato per altri motivi
		}

		return false;
	}

	public bool hasBeenBought(){

		if (PlayerPrefs.GetInt ("Isbought" + id, 0) == 1)
			return true;
		else
			return false;
	}
}
