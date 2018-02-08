using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerManager : MonoBehaviour {

	public Sprite[] flowerSprites;

	public Sprite getFlowerSprite(){
		return flowerSprites[Random.Range(0, flowerSprites.Length - 1)];
	}
}
