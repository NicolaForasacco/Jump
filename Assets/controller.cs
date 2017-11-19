using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour {

	float height=25;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public float gameHeight(){
		return height;
	}

	public void setNewHeight(float newHeight){
		height = newHeight;
	}
}
