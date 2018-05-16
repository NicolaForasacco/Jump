using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour {

	public Sprite[] startMessages;

	public Sprite getMessage(){
		return startMessages[Random.Range(0, startMessages.Length - 1)];
	}
}
