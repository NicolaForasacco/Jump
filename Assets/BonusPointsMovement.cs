using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPointsMovement : MonoBehaviour {
	
	public int speedX, speedY;

	private int sX, sY;

	void Start () {
		
		sX = Random.Range (0, 3) * speedX;
		sY = - Random.Range (0, 3) * speedY;

		lastUpdatePosition = new Vector3 (transform.position.x, transform.position.y,transform.position.z);
	}

	Vector3 lastUpdatePosition;

	void Update () {
		
		lastUpdatePosition.x = lastUpdatePosition.x + sX * Time.deltaTime;
		lastUpdatePosition.y = lastUpdatePosition.y + sY * Time.deltaTime;

		transform.position = lastUpdatePosition;
		
	}
}
