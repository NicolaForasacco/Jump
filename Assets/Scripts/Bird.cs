using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

	float speedX = 100;
	float speedY = 200;

	int side = 1, deleteWhenPlayerFurtherThan = 400, startWhenPlayerIsCloserThan = 100;

	bool isMoving = false;

	GameObject player;

	Vector3 initialPosition;


	public float minScale = 0.3f, maxScale = 0.8f;

	void Start () {

		player = GameObject.FindGameObjectWithTag ("player");
		transform.SetParent (GameObject.FindGameObjectWithTag ("foreground").transform);

		side = (GameObject.FindGameObjectWithTag ("GameController").GetComponent<SpawnTileManager> ().getAverageX() > 0) ? 1 : -1;

		float randomScale = Random.Range (minScale, maxScale);

		transform.localScale = new Vector2 (side * randomScale, 1 * randomScale);
		initialPosition = transform.position;
	}

	void Update () {
		
		if (isMoving) {
			
			transform.position += Vector3.right * side * Time.deltaTime * speedX;
			float r = Random.value;

			if (r > 0.2f)
				transform.position += Vector3.up * Time.deltaTime * speedY;
			else if (r > 0f)
				transform.position += Vector3.down * Time.deltaTime * speedY * 0.8f;

			if ((transform.position.x >= initialPosition.x + deleteWhenPlayerFurtherThan ||
				transform.position.x <= initialPosition.x - deleteWhenPlayerFurtherThan) && 
				transform.position.y >= initialPosition.y + deleteWhenPlayerFurtherThan)
				DestroyObject (this.gameObject);
		} else {
			
			if (player != null) {
				if (transform.position.y - player.transform.position.y < startWhenPlayerIsCloserThan
					&& transform.position.x - player.transform.position.x < startWhenPlayerIsCloserThan)
					isMoving = true;
			}
		}
	}
}
