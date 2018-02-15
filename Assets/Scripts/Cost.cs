using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cost  : MonoBehaviour {

	public int count;
	public objectType type;
	public bool boughtWithObjects; // se donut o diamanti o qualsiasi altro oggetto

	public enum objectType{ donut,pizza,hamburger};

	public int getCount(){
		return count;
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
}
