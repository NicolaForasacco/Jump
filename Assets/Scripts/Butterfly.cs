using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour {

	private GameObject player;
	private Vector3 initialPosition;

	private int side = 1, startWhenPlayerIsCloserThan = 100, deleteWhenPlayerFurtherThan = 400;
	private float speedX = 100, speedY = 200;
	private bool isMoving = false;

	public float minScale = 0.4f, maxScale = 0.9f;

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

			if (r > 0.3f)
				transform.position += Vector3.up * Time.deltaTime * speedY;
			else if (r > 0f)
				transform.position += Vector3.down * Time.deltaTime * speedY * 0.8f;

			if ((transform.position.x >= initialPosition.x + deleteWhenPlayerFurtherThan || 
				transform.position.x <= initialPosition.x - deleteWhenPlayerFurtherThan) && 
				transform.position.y >= initialPosition.y + deleteWhenPlayerFurtherThan)
				DestroyObject (this.gameObject);
			
		} else {

			if (player != null) {
				if (transform.position.y - player.transform.position.y < startWhenPlayerIsCloserThan && transform.position.x - player.transform.position.x < startWhenPlayerIsCloserThan)
					isMoving = true;
			}
		}
	}
}

