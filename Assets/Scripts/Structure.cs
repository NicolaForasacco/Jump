using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour {

	GameObject player;

	Vector3 initialPosition;

	void Start () {

		player = GameObject.FindGameObjectWithTag ("player");

		if(transform.parent == null)
			transform.SetParent (GameObject.FindGameObjectWithTag ("foreground").transform);
	}

	bool visited = false;

	bool gone = false;

	void Update () {

		if (player != null) {
			
			if (transform.position.y - player.transform.position.y < -200)
				Destroy (this.gameObject);
			else if (transform.position.y - player.transform.position.y < -40) {
				
				if (!gone) {
					player.GetComponent<PlayerMovement> ().hideMessage ();
					gone = true;
				}
			} else if (transform.position.y - player.transform.position.y < 100 && transform.position.x - player.transform.position.x < 100 && !visited) {
				player.GetComponent<PlayerMovement> ().showFinishLineMessage ();
				visited = true;
			}
		}
	}
}
