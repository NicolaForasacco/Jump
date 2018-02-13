using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cost  : MonoBehaviour {

	public int count;
	public objectType type;
	public bool boughtWithObjects; // se donut o diamanti o qualsiasi altro oggetto

	public enum objectType{ donut, diamond};

	public int getCount(){
		return count;
	}

	public string getType(){
		
		switch(type){
		case objectType.donut:
			return "Donuts";

		case objectType.diamond:
			return "Diamonds";

		default:
			return "";
		}

	}
}
