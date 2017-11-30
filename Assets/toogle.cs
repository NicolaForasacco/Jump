using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toogle : MonoBehaviour {
	Image button;
	public Sprite s1;
	public Sprite s2;
	int count = 0;


	// Use this for initialization
	void Start () {
		button = (this.GetComponent<Button>()).GetComponent<Image>();

	}
	
	public void changeButton(){
		count++;

		if (count % 2 == 0)
			button.sprite = s1;
		else
			button.sprite = s2;
	}
}
