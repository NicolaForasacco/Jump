using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour {

	public Sprite[] startMessages;
	public Sprite[] finishLineMessages;

	public Sprite getStartMessage(){
		return startMessages[Random.Range(0, startMessages.Length - 1)];
	}

	public Sprite getFinishLineMessage(){
		return finishLineMessages[Random.Range(0, finishLineMessages.Length - 1)];
	}
}
