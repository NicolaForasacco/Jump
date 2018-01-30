using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

	int side = 1;
	float speedX = 100;
	float speedY = 200;

	bool isMoving = false;

	GameObject player;

	void Start () {

		player = GameObject.FindGameObjectWithTag ("player");
		transform.SetParent (GameObject.FindGameObjectWithTag ("foreground").transform);

		side = (GameObject.FindGameObjectWithTag ("GameController").GetComponent<SpawnTile> ().lastXPosition > 0) ? 1 : -1;

		transform.localScale = new Vector2 (side, 1);

		transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
	}

	void Update () {
		
		if (isMoving) {
			
			transform.position += Vector3.right * side * Time.deltaTime * speedX;
			float r = Random.value;

			if (r > 0.2f)
				transform.position += Vector3.up * Time.deltaTime * speedY;
			else if (r > 0f)
				transform.position += Vector3.down * Time.deltaTime * speedY * 0.8f;

			if ((transform.position.x >= 250 || transform.position.x <= -250) && transform.position.y >= 250)
				DestroyObject (this.gameObject);
		} else {
			
			if (player != null) {
				if (transform.position.y - player.transform.position.y < 100 && transform.position.x - player.transform.position.x < 100)
					startMoving (0.1f);	
			}
		}
	}

	private void startMoving(float waitFor){
		
		StartCoroutine (startMovingCoroutine(waitFor));
	}

	private IEnumerator startMovingCoroutine(float time){

		yield return new WaitForSeconds(time);
		isMoving = true;
	}
}
