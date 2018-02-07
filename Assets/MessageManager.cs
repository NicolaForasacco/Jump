using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour {

	public Sprite[] startMessages;

	// Use this for initialization
	void Start () {
		
	}

	public Sprite getStartMessage(){
		return startMessages[Random.Range(0,startMessages.Length - 1)];
	}

	public Sprite getFinishLineMessage(){
		return startMessages[Random.Range(0,startMessages.Length - 1)];
	}

	// Update is called once per frame
	void Update () {
		
	}
}
