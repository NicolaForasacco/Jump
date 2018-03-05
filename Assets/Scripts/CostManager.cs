using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostManager : MonoBehaviour {

	public Cost cost;
	public int characterId;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	int donutsPicked;

	public bool buy(){



		if ( canBeBought() && PlayerPrefs.GetInt ("Isbought" + characterId, 0) == 0) {

			PlayerPrefs.SetInt("Isbought" + characterId, 1);

			int objectsPicked = PlayerPrefs.GetInt (cost.getType(), 0);
			objectsPicked -= cost.getCount();

			PlayerPrefs.SetInt (cost.getType(), objectsPicked);

			return true;
		} else if (PlayerPrefs.GetInt ("Isbought" + characterId, 0) == 1)
			return true;
		else
			return false;
		
	}

	public bool canBeBought(){
		
		if (cost != null) {
			if (cost.boughtWithObjects && cost.getCount () <= PlayerPrefs.GetInt (cost.getType (), 0) && !hasBeenBought())
				return true;
			else {
				// guardare se può essere sbloccato per altri motivi
			}
		}

		return false;
	}

	public bool hasBeenBought(){
		
		if (PlayerPrefs.GetInt ("Isbought" + characterId, 0) == 1)
			return true;
		else
			return false;
	}
}
