using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostManager : MonoBehaviour {

	public int cost = 50;
	public int characterId = 1;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	int donutsPicked;

	public bool buy(){

		donutsPicked = PlayerPrefs.GetInt ("Donuts", 0);

		if (donutsPicked >= cost && PlayerPrefs.GetInt ("Is" + characterId + "bought", 0) == 0) {

			PlayerPrefs.SetInt("Is" + characterId + "bought", 1);

			donutsPicked -= cost;
			PlayerPrefs.SetInt ("Donuts", donutsPicked);

			return true;
		} else if (PlayerPrefs.GetInt ("Is" + characterId + "bought", 0) == 1)
			return true;
		else
			return false;
		
	}

	public bool hasBeenBought(){
		
		if (PlayerPrefs.GetInt ("Is" + characterId + "bought", 0) == 1)
			return true;
		else
			return false;
	}
}
