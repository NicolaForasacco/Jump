using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalLift : MonoBehaviour {

	public float speed = 10;
	public bool isOn = true;

	GameObject player;

	void Start(){
		
		player = GameObject.FindGameObjectWithTag ("player");

		StartCoroutine (turnOffLift(8));

	}
	// Update is called once per frame
	void Update () {

		transform.position += Vector3.up * speed * Time.deltaTime;

		if (player != null)
			if (player.transform.position.y - transform.position.y > 300)
				DestroyObject (this.gameObject);
		
		if (!isOn)
			speed -= 15 * Time.deltaTime;
	}



	private IEnumerator turnOffLift(float time){

		yield return new WaitForSeconds(time);
		GetComponent<Animator> ().SetTrigger ("TurnOff");
		isOn = false;
	}
}
