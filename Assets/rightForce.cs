using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rightForce : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D> ().AddForce(new Vector2(10000, 0),ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
