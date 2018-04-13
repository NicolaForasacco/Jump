using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTileController : MonoBehaviour {
	
	public float horizontalSpeed = 20f;
	public int horizontalWide = 20;

	public int right = 0;
	public bool side = true;
	public float maxSideSpeed = 2f;

	void Start(){
		
		side = (Random.value > 0.5f);
		horizontalSpeed = horizontalSpeed * Random.Range (1, maxSideSpeed);
	}

	// Update is called once per frame
	void Update () {

		if (side) {
			if (right < horizontalWide) {
				transform.position += Vector3.right * horizontalSpeed * Time.deltaTime;

				right++;
			} else {
				side = !side;
				right--;
			}
		}
		else {
			if (right > -horizontalWide) {
				transform.position -= Vector3.right * horizontalSpeed * Time.deltaTime;

				right--;
			} else {
				side = !side;
				right++;
			}
		}
	}
}
