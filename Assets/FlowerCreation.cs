using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerCreation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		if (Random.value > 0.5)
			transform.localScale = new Vector2 (1, 1);
		else
			transform.localScale = new Vector2 (-1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
