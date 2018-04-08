using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour {

	public Sprite[] cloudSprites;

	public Sprite getCloudSprite(){
		return cloudSprites[Random.Range(0, cloudSprites.Length - 1)];
	}
}
