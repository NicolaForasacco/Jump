using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAnimal : MonoBehaviour {

	private GameObject player;
	private Vector3 initialPosition;

	private int side = 1, deleteWhenPlayerFurtherThan = 400, startWhenPlayerIsCloserThan = 100;
	private float speedX = 100, speedY = 200;
	private bool isMoving = false;


	public float minScale = 0.3f, maxScale = 0.8f, upP = 0.2f;

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

			if (r > upP)
				transform.position += Vector3.up * Time.deltaTime * speedY;
			else if (r > 0f)
				transform.position += Vector3.down * Time.deltaTime * speedY * 0.8f;

			if ((transform.position.x >= initialPosition.x + deleteWhenPlayerFurtherThan ||
				transform.position.x <= initialPosition.x - deleteWhenPlayerFurtherThan) && 
				transform.position.y >= initialPosition.y + deleteWhenPlayerFurtherThan)
				DestroyObject (this.gameObject);
			
		} else {
			
			if (player != null) {
				if (transform.position.y - player.transform.position.y < startWhenPlayerIsCloserThan && 
					transform.position.x - player.transform.position.x < startWhenPlayerIsCloserThan)
					isMoving = true;
			}
		}
	}
}
